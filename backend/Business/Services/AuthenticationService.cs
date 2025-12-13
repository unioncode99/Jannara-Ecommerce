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
using Microsoft.Extensions.Configuration;

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
        private readonly IConfirmationTokenServiceInterface _confirmationTokenService;
        private readonly IConfiguration _configuration;
        private readonly IEmailSenderService _emailSenderService;
        private readonly ILogger<ICustomerRepository> _logger;
        private readonly ICodeService _codeService;

        public AuthenticationService(IUserService userService, IPasswordService passwordService, ITokenService tokenService, IRefreshTokenService refreshTokenService, 
            IPersonService personService, ICustomerService customerService, ISellerService sellerService, IConfirmationTokenServiceInterface confirmationTokenService,
            IConfiguration configuration, IEmailSenderService emailSenderService, ILogger<ICustomerRepository> logger,
            ICodeService codeService)
        {
            _userService = userService;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _personService = personService;
            _customerService = customerService;
            _SellerService = sellerService;
            _confirmationTokenService = confirmationTokenService;
            _configuration = configuration;
            _emailSenderService = emailSenderService;
            _logger = logger;
            _codeService = codeService;
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
            string token = _tokenService.GenerateResetToken();
            string code = _codeService.GenerateCode(4);

            ConfirmationTokenDTO resetPasswordDTO = new ConfirmationTokenDTO(0, userResult.Data.Id, token, code, DateTime.Now.AddMinutes(15), false);
            Result<int> saveTokenResult = await _confirmationTokenService.AddNewAsync(resetPasswordDTO);
            if (!saveTokenResult.IsSuccess)
            {
                return new Result<bool>(false, "internal_server_error", false, 500);
            }

            string resetUrl = $"{_configuration.GetValue<string>("EMAIL_CONFIGURATION:FRONTEND_DOMAIN")}/reset-password/{token}";
            string body = $@"
<html>
<head>
  <style>
    body {{
      font-family: Arial, sans-serif;
      background-color: #f4f4f4;
      margin: 0;
      padding: 0;
    }}
    .container {{
      max-width: 600px;
      margin: 40px auto;
      background-color: #ffffff;
      border-radius: 8px;
      overflow: hidden;
      box-shadow: 0 4px 12px rgba(0,0,0,0.1);
    }}
    .header {{
      background-color: #e89a0a;
      color: #fff;
      text-align: center;
      padding: 20px;
      font-size: 24px;
      font-weight: bold;
    }}
    .content {{
      padding: 30px 20px;
      color: #333;
      font-size: 16px;
      line-height: 1.6;
    }}
    .code {{
      display: inline-block;
      background-color: #e89a0a;
      color: #fff;
      font-weight: bold;
      font-size: 20px;
      padding: 10px 20px;
      border-radius: 6px;
      letter-spacing: 2px;
      text-align: center;
      margin: 20px 0;
    }}
    .button {{
      display: inline-block;
      background-color: #e89a0a;
      color: #fff !important;
      text-decoration: none;
      padding: 12px 24px;
      border-radius: 6px;
      font-weight: bold;
      margin-top: 20px;
    }}
    .footer {{
      padding: 20px;
      font-size: 12px;
      color: #777;
      text-align: center;
    }}
  </style>
</head>
<body>
  <div class='container'>
    <div class='header'>Jannara App</div>
    <div class='content'>
      <p>Hello,</p>
      <p>You recently requested to reset your password. You can use the code below or click the button to reset your password:</p>
      <div class='code'>{code}</div>
      <p style='text-align:center;'>
        <a href='{resetUrl}' class='button'>Reset Password</a>
      </p>
      <p>If you did not request a password reset, you can safely ignore this email.</p>
      <p>Thanks,<br/>The Jannara Team</p>
    </div>
    <div class='footer'>
      &copy; {DateTime.UtcNow.Year} Jannara. All rights reserved.
    </div>
  </div>
</body>
</html>";

            try
            {
                await _emailSenderService.SendEmailAsync(email, "RESET PASSWORD", body);
                return new Result<bool>(true, "reset-link-sent", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Result<bool>(false, "internal_server_error", false, 500);
            }
            
        }
    
    }
}
