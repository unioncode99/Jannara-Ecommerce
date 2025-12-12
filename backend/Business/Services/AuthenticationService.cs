using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.Authentication;
using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Enums;
using Jannara_Ecommerce.Mappers;
using Jannara_Ecommerce.Utilities;
using Jannara_Ecommerce.Utilities.WrapperClasses;

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

        public AuthenticationService(IUserService userService, IPasswordService passwordService, ITokenService tokenService, IRefreshTokenService refreshTokenService, 
            IPersonService personService, ICustomerService customerService, ISellerService sellerService)
        {
            _userService = userService;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _personService = personService;
            _customerService = customerService;
            _SellerService = sellerService;
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
    }
}
