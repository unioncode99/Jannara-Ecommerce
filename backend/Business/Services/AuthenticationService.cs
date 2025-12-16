using Azure.Core;
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
using System.Data.Common;
using System.Transactions;

namespace Jannara_Ecommerce.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IPersonService _personService;
        private readonly IConfirmationTokenService _confirmationTokenService;
        private readonly ILogger<ICustomerRepository> _logger;
        private readonly string _connectionString;
        private readonly IConfirmationService _confirmationService;

        public AuthenticationService(IUserService userService, IPasswordService passwordService, ITokenService tokenService, IRefreshTokenService refreshTokenService, IPersonService personService, IConfirmationTokenService confirmationTokenService,
            ILogger<ICustomerRepository> logger, IOptions<DatabaseSettings> dateBaseSettings, IConfirmationService confirmationService)
        {
            _userService = userService;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _personService = personService;
            _confirmationTokenService = confirmationTokenService;
            _logger = logger;
            _connectionString = dateBaseSettings.Value.DefaultConnection;
            _confirmationService = confirmationService;
        }

        private async Task<Result<GenerateLoginInfoResult>> _generateLoginInfo(UserDTO user, PersonDTO person)
        {
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenResult = await _refreshTokenService.AddNewAsync(user.Id, refreshToken, DateTime.UtcNow.AddDays(7));
            if (!refreshTokenResult.IsSuccess)
                return new Result<GenerateLoginInfoResult>(false, refreshTokenResult.Message, null, refreshTokenResult.ErrorCode);
            var loginResponse = new LoginResponseDTO(person, user.ToUserPublicDTO(), accessToken);
            var result = new GenerateLoginInfoResult(loginResponse, refreshToken);
            return new Result<GenerateLoginInfoResult>(true, "login_info_generated_successfully", result);
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

            var loginInfoResult = await _generateLoginInfo(userResult.Data, personResult.Data);
            if (!loginInfoResult.IsSuccess)
                return new Result<LoginResult>(false, loginInfoResult.Message, null, loginInfoResult.ErrorCode);

            var loginResult = new LoginResult(loginInfoResult.Data.LoginResponse, loginInfoResult.Data.RefreshToken);

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
            var validatingTokenResult = await _confirmationService.ValidateTokenAsync(resetPasswordDTO.Token);

            if (!validatingTokenResult.IsSuccess)
            {
                return new Result<bool>(false, "invalid_or_expired_token", false, 401);

            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    await connection.OpenAsync();
                    transaction = (SqlTransaction)await connection.BeginTransactionAsync();
                    Result<bool> resetPasswordResult = await _userService.ResetPasswordAsync(validatingTokenResult.Data, resetPasswordDTO.NewPassword, connection, transaction);
                    if (!resetPasswordResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return new Result<bool>(false, resetPasswordResult.Message, false, resetPasswordResult.ErrorCode);
                    }
                    Result<bool> markAsUsed = await _confirmationTokenService.MarkAsUsedAsync(validatingTokenResult.Data, connection, transaction);
                    if (!markAsUsed.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return new Result<bool>(false, markAsUsed.Message, false, markAsUsed.ErrorCode);
                    }
                    await transaction.CommitAsync();
                    return resetPasswordResult;
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                        try
                        {
                            await transaction.RollbackAsync();
                        }
                        catch (Exception rollBackEx)
                        {
                            _logger.LogError(rollBackEx, "failed to roll back while  reset password for UserID {UserID}", validatingTokenResult.Data.);
                        }
                    _logger.LogError(ex, "Failed to reset password for UserId {UserID}", validatingTokenResult.Data);
                    return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                }

            }
        }

        public async Task<Result<LoginResult>> ConfirmAccountAsync(string token)
        {
            Result<ConfirmationTokenDTO> confirmationTokenResult = await _confirmationTokenService.GetByTokenAsync(token);
            if (!confirmationTokenResult.IsSuccess && confirmationTokenResult.ErrorCode == 404)
                return new Result<LoginResult>(false, "invalid_or_expired_token", null, 401);

            if (!confirmationTokenResult.IsSuccess)
                return new Result<LoginResult>(false, confirmationTokenResult.Message, null, confirmationTokenResult.ErrorCode);
            if (confirmationTokenResult.Data.ExpireAt < DateTime.Now || confirmationTokenResult.Data.IsUsed)
                return new Result<LoginResult>(false, "invalid_or_expired_token", null, 401);

            var getUserTask = _userService.FindAsync(confirmationTokenResult.Data.UserId);
            var markTokenAsUsedTask = _confirmationTokenService.MarkAsUsedAsync(confirmationTokenResult.Data.UserId);
            var markEmailAsConfirmedTask = _userService.MarkEmailAsConfirmed(confirmationTokenResult.Data.UserId);
            await Task.WhenAll(markEmailAsConfirmedTask, markTokenAsUsedTask, getUserTask);

            if (!markTokenAsUsedTask.Result.IsSuccess)
                return new Result<LoginResult>(false, markTokenAsUsedTask.Result.Message, null, markTokenAsUsedTask.Result.ErrorCode);
            if (!markEmailAsConfirmedTask.Result.IsSuccess)
                return new Result<LoginResult>(false, markEmailAsConfirmedTask.Result.Message, null, markEmailAsConfirmedTask.Result.ErrorCode);
            if (!getUserTask.Result.IsSuccess)
                return new Result<LoginResult>(false, getUserTask.Result.Message, null, getUserTask.Result.ErrorCode);

            var personResult = await _personService.FindAsync(getUserTask.Result.Data.PersonId);
            if (!personResult.IsSuccess)
                return new Result<LoginResult>(false, personResult.Message, null, personResult.ErrorCode);

            var loginInfoResult = await _generateLoginInfo(getUserTask.Result.Data, personResult.Data);
            if (!loginInfoResult.IsSuccess)
                return new Result<LoginResult>(false, loginInfoResult.Message, null, loginInfoResult.ErrorCode);

            var loginRsult = new LoginResult(loginInfoResult.Data.LoginResponse, loginInfoResult.Data.RefreshToken);
            return new Result<LoginResult>(true, "verification_success", loginRsult);

        }

        public async Task<Result<bool>> ResendAccountConfirmationAsync(string email)
        {
            Result<UserDTO> userResult = await _userService.FindAsync(email);
            if (!userResult.IsSuccess)
            {
                return new Result<bool>(false, userResult.Message, false, userResult.ErrorCode);
            }
            if (userResult.Data.IsConfirmed)
            {
                return new Result<bool>(false, "Account already confirmed", false, 400);
            }

            var sendingConfirmationEmailResult = await _confirmationService.SendAccountConfirmationAsync(userResult.Data.ToUserPublicDTO());
            if (!sendingConfirmationEmailResult.IsSuccess)
            {
                return new Result<bool>(false, "internal_server_error", false, 500);
            }

            return new Result<bool>(true, "Confirmation link/code resent successfully", true, 200);
        }

        public async Task<Result<VerifyCodeResposeDTO>> VerifyCodeAsync(string code)
        {
            Result<ConfirmationTokenDTO> getCodeResult = await _confirmationTokenService.GetByCodeAsync(code);
            if (!getCodeResult.IsSuccess && getCodeResult.ErrorCode == 404)
                return new Result<VerifyCodeResposeDTO>(false, "invalid_or_expired_code", null, 401);
            if (!getCodeResult.IsSuccess)
                return new Result<VerifyCodeResposeDTO>(false, getCodeResult.Message, null, getCodeResult.ErrorCode);
            if (getCodeResult.Data.ExpireAt < DateTime.Now || getCodeResult.Data.IsUsed)
                return new Result<VerifyCodeResposeDTO>(false, "invalid_or_expired_code", null, 401);

            var respponse = new VerifyCodeResposeDTO(getCodeResult.Data.Purpose, getCodeResult.Data.Token);
            return new Result<VerifyCodeResposeDTO>(true, "verification_success", respponse);

        }

        public async Task<Result<bool>> ResendVerificationCodeAsync(string email)
        {
            var getConfirmationTokenResult = await _confirmationTokenService.GetByEmailAsync(email);
            if (!getConfirmationTokenResult.IsSuccess && getConfirmationTokenResult.ErrorCode != 404)
                return new Result<bool>(false, getConfirmationTokenResult.Message, false, getConfirmationTokenResult.ErrorCode);
            if (!getConfirmationTokenResult.IsSuccess && getConfirmationTokenResult.ErrorCode == 404)
                return new Result<bool>(true, "if an account exists, a code has been sent to your email.", true);
            var getUserResult = await _userService.FindAsync(email);
            if (!getUserResult.IsSuccess)
                return new Result<bool>(false, getUserResult.Message, false, getUserResult.ErrorCode);

            if (getConfirmationTokenResult.Data.Purpose == ConfirmationPurpose.ResetPassword)
            {
                var markAsUserResult = await _confirmationTokenService.MarkAsUsedAsync(getUserResult.Data.Id);
                var sendForgetPasswordResult = await _confirmationService.SendForgetPasswordConfirmationAsync(getUserResult.Data);
                if (!sendForgetPasswordResult.IsSuccess)
                    return new Result<bool>(false, sendForgetPasswordResult.Message, false, sendForgetPasswordResult.ErrorCode);
                return new Result<bool>(true, "successfully_resend_verification_code", true);
            }
            if (getConfirmationTokenResult.Data.Purpose == ConfirmationPurpose.VerifyEmail)
            {
                var markAsUserResultResult = await _confirmationTokenService.MarkAsUsedAsync(getUserResult.Data.Id);
                var sendConfirmAccountResult = await _confirmationService.SendAccountConfirmationAsync(getUserResult.Data.ToUserPublicDTO());

                if (!sendConfirmAccountResult.IsSuccess)
                    return new Result<bool>(false, sendConfirmAccountResult.Message, false, sendConfirmAccountResult.ErrorCode);
                return new Result<bool>(true, "successfully_resend_verification_code", true);
            }

            throw new Exception("Unspported Type of confirmatation code");
        }
    }
}
