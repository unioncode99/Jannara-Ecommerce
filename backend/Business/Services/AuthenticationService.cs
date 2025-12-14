using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Authentication;
using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.DTOs.Token;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Enums;
using Jannara_Ecommerce.Mappers;
using Jannara_Ecommerce.Utilities;
using Jannara_Ecommerce.Utilities.WrapperClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IPersonService _personService;
        private readonly ICustomerService _customerService;
        private readonly ISellerService _SellerService;
        private readonly IConfirmationTokenService _confirmationTokenService;
        private readonly IConfiguration _configuration;
        private readonly IEmailSenderService _emailSenderService;
        private readonly ILogger<ICustomerRepository> _logger;
        private readonly ICodeService _codeService;
        private readonly string _connectionString;
        private readonly IConfirmationService _confirmationService;

        public AuthenticationService(IUserService userService, IPasswordService passwordService, ITokenService tokenService, IRefreshTokenService refreshTokenService, 
            IPersonService personService, ICustomerService customerService, ISellerService sellerService, IConfirmationTokenService confirmationTokenService,
            IConfiguration configuration, IEmailSenderService emailSenderService, ILogger<ICustomerRepository> logger,
            ICodeService codeService, IOptions<DatabaseSettings> dateBaseSettings,
            IConfirmationService confirmationService)
        {
            _userService = userService;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _personService = personService;
            _confirmationTokenService = confirmationTokenService;
            _configuration = configuration;
            _emailSenderService = emailSenderService;
            _logger = logger;
            _codeService = codeService;
            _connectionString = dateBaseSettings.Value.DefaultConnection;
            _confirmationService = confirmationService;
        }

        public async Task<Result<LoginResult>> LogInAsync(LoginDTO request)
        {
            var userResult = await _userService.FindAsync(request.Email);
            if (!userResult.IsSuccess)
            {
                if (userResult.ErrorCode == 404)
                    return new Result<LoginResult>(false, "invalid_username/password", null, 401);
                return new Result<LoginResult>(false, userResult.Message, null, userResult.ErrorCode);
            }
            if (!userResult.Data.Roles.Any(r => r.IsActive))
                return new Result<LoginResult>(false, "user_disabled", null, 401);
            if (!_passwordService.VerifyPassword<UserDTO>(userResult.Data, userResult.Data.Password, request.Password))
            {
                return new Result<LoginResult>(false, "Invalid username/password", null, 401);
            }

            Result<PersonDTO> personResult = await _personService.FindAsync(userResult.Data.PersonId);
            if (!personResult.IsSuccess)
            {
                return new Result<LoginResult>(false, personResult.Message, null, personResult.ErrorCode);
            }
            

            var accessToken = _tokenService.GenerateAccessToken(userResult.Data);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenResult = await _refreshTokenService.AddNewAsync(userResult.Data.Id, refreshToken, DateTime.UtcNow.AddDays(7));
            if (!refreshTokenResult.IsSuccess)
                return new Result<LoginResult>(false, refreshTokenResult.Message, null, refreshTokenResult.ErrorCode);
            var loginResponse = new LoginResponseDTO(personResult.Data, userResult.Data.ToUserPublicDTO(), accessToken);

            var loginResult = new LoginResult(loginResponse, refreshToken);

            return new Result<LoginResult>(true, "Successfuly Logged in", loginResult);
        }

        public async Task<Result<bool>> ForgetPasswordAsync(string email)
        {
            Result<UserDTO> userResult = await _userService.FindAsync(email);
            if (!userResult.IsSuccess)
            {
                return new Result<bool>(false, userResult.Message, false, userResult.ErrorCode);
            }
            var sendingForgetPasswordConfirmationResult = _confirmationService.SendForgetPasswordConfirmationAsync(userResult.Data);
            if (!sendingForgetPasswordConfirmationResult.Result.IsSuccess)
            {
                return new Result<bool>(false, "internal_server_error", false, 500);
            }

            return new Result<bool>(true, "reset_link_sent", true);
        }

        public async Task<Result<bool>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            if ((resetPasswordDTO.Code != null && resetPasswordDTO.Token != null) ||
                (resetPasswordDTO.Code == null && resetPasswordDTO.Token == null))
            {
                return new Result<bool>(false, "invalid data", false, 400);
            }

            Result<int> validatingTokenResult;

            if (!string.IsNullOrWhiteSpace(resetPasswordDTO.Code))
            {
                validatingTokenResult = await _confirmationService.ValidateCodeAsync(resetPasswordDTO.Code);
            }
            else
            {
                validatingTokenResult = await _confirmationService.ValidateTokenAsync(resetPasswordDTO.Token);
            }

            if (!validatingTokenResult.IsSuccess || validatingTokenResult.Data <= 0)
            {
                return new Result<bool>(false, "Invalid or expired code/token", false, 400);

            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    await connection.OpenAsync();
                    transaction = connection.BeginTransaction();
                    Result<bool> resetPasswordResult = await _userService.ResetPasswordAsync(validatingTokenResult.Data, resetPasswordDTO.NewPassword, connection, transaction);
                    if (!resetPasswordResult.IsSuccess)
                    {
                        transaction.Rollback();
                        return new Result<bool>(false, resetPasswordResult.Message, false, resetPasswordResult.ErrorCode);
                    }
                    Result<bool> markAsUsed = await _confirmationTokenService.MarkAsUsedAsync(validatingTokenResult.Data, connection, transaction);
                    if (!markAsUsed.IsSuccess)
                    {
                        transaction.Rollback();
                        return new Result<bool>(false, markAsUsed.Message, false, markAsUsed.ErrorCode);
                    }
                    await transaction.CommitAsync();
                    return resetPasswordResult;
                }
                catch (Exception ex)
                {
                    {
                        transaction.Rollback();
                        _logger.LogError(ex.Message);
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }
                }

            }
        }

        public async Task<Result<string>> VerifyResetCodeAsync(string resetCode)
        {
            Result<ConfirmationTokenDTO> result = await _confirmationTokenService.GetByCodeAsync(resetCode);
            if (!result.IsSuccess && result.ErrorCode == 404)
                return  new Result<string>(false, "invalid_or_expired_code", "", 401);
            if (!result.IsSuccess)
                return new Result<string>(false,result.Message, "", result.ErrorCode);
            if (result.Data.ExpireAt > DateTime.Now || result.Data.IsUsed)
                new Result<string>(false, "invalid_or_expired_code", "", 401);

            return new Result<string>(true, "verification_success", result.Data.Token);

        }
    }
}
