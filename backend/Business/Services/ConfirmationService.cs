using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Authentication;
using Jannara_Ecommerce.DTOs.Token;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Enums;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Jannara_Ecommerce.Business.Services
{
    public class ConfirmationService : IConfirmationService
    {
        private readonly IUserConfirmationService _userConfirmationService;
        private readonly IConfirmationTokenService _confirmationTokenService;
        private readonly ITokenService _tokenService;
        private readonly ICodeService _codeService;
        private readonly IConfiguration _configuration;
        private readonly IEmailSenderService _emailSenderService;
        private readonly ILogger<IConfirmationService> _logger;
        public ConfirmationService(IUserConfirmationService userConfirmationService, IConfirmationTokenService confirmationTokenService, 
            ITokenService tokenService, ICodeService codeService, 
            IConfiguration configuration, IEmailSenderService emailSenderService,
            ILogger<IConfirmationService> logger)
        {
            _userConfirmationService = userConfirmationService;
            _confirmationTokenService = confirmationTokenService;
            _tokenService = tokenService;
            _codeService = codeService;
            _configuration = configuration;
            _emailSenderService = emailSenderService;
            _logger = logger;
        }

        public async Task<Result<bool>> SendAccountConfirmationAsync(UserPublicDTO userInfo)
        {
            var confirmationTokenResult = await CreateAndSaveConfirmationTokenAsync(userInfo.Id, ConfirmationPurpose.VerifyEmail, 15 );
            if (!confirmationTokenResult.IsSuccess)
            {
                return new Result<bool>(false, "internal_server_error", false, 500);
            }
            string confirmUrl = $"{_configuration.GetValue<string>("EMAIL_CONFIGURATION:FRONTEND_DOMAIN")}/confirm-account?token={confirmationTokenResult.Data.Token}";
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
      <p>Welcome! Please confirm your account using the code below or by clicking the button:</p>
      <div class='code'>{confirmationTokenResult.Data.Code}</div>
      <p style='text-align:center;'>
        <a href='{confirmUrl}' class='button'>Confirm Account</a>
      </p>
      <p>If you did not create an account, you can safely ignore this email.</p>
      <p>Thanks,<br/>The Jannara Team</p>
    </div>
    <div class='footer'>
      &copy; {DateTime.UtcNow.Year} Jannara. All rights reserved.
    </div>
  </div>
</body>
</html>";
            var sendingConfirmationEmailResult = await SendConfirmationEmailAsync(userInfo.Email, "Account Confirmation", body, "account_confirmation_link_sent");
            if (!sendingConfirmationEmailResult.Data)
            {
                return new Result<bool>(false, "internal_server_error", false, 500);
            }
            return new Result<bool>(true, "account_confirmation_link_sent", true);
        }

        public async Task<Result<bool>> SendForgetPasswordConfirmationAsync(UserDTO userInfo)
        {
            var confirmationTokenResult = await CreateAndSaveConfirmationTokenAsync(userInfo.Id, ConfirmationPurpose.ResetPassword, 15);
            if (!confirmationTokenResult.IsSuccess)
            {
                return new Result<bool>(false, "internal_server_error", false, 500);
            }
            string resetUrl = $"{_configuration.GetValue<string>("EMAIL_CONFIGURATION:FRONTEND_DOMAIN")}/reset-password?token={confirmationTokenResult.Data.Token}";
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
      <div class='code'>{confirmationTokenResult.Data.Code}</div>
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
            var sendingConfirmationEmailResult = await SendConfirmationEmailAsync(userInfo.Email, "RESET PASSWORD", body, "reset_link_sent");
            if (!sendingConfirmationEmailResult.Data)
            {
                return new Result<bool>(false, "reset_link_sent", false, 500);
            }
            return new Result<bool>(true, "account_confirmation_link_sent", true);
        }

        public async Task<Result<ConfirmationTokenDTO>> CreateAndSaveConfirmationTokenAsync(int UserId,
             ConfirmationPurpose purpose, int MinutesToExpires = 15, string successMessage = "token_created_successfully")
        {
            string token = _tokenService.GenerateResetToken();
            string code = _codeService.GenerateCode(4);

            var confirmationTokenDto = new ConfirmationTokenDTO(
                0,
                UserId,
                token,
                code,
                purpose,
                DateTime.Now.AddMinutes(MinutesToExpires),
                false
            );

            Result<int> saveTokenResult =
                await _confirmationTokenService.AddNewAsync(
                    confirmationTokenDto
                );

            if (!saveTokenResult.IsSuccess)
            {
                return new Result<ConfirmationTokenDTO>(false, "internal_server_error", null, 500);
            }

            confirmationTokenDto.Id = saveTokenResult.Data;

            return new Result<ConfirmationTokenDTO>(true, successMessage, confirmationTokenDto, 200);
        }

        public async Task<Result<bool>> SendConfirmationEmailAsync(string toEmail, string subject, string body, string successMessage = "link_was_sent")
        {
            try
            {
                await _emailSenderService.SendEmailAsync(toEmail, subject, body);
                return new Result<bool>(true, successMessage, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email. Subject: {Subject}, To: {To}", subject, toEmail);
                return new Result<bool>(false, "internal_server_error", false, 500);
            }
        }

        public async Task<Result<int>> ValidateTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return new Result<int>(false, "Token is required", -1, 400);
            }

            var confirmationTokenResult = await _confirmationTokenService
                    .GetByTokenAsync(token);


            if (!confirmationTokenResult.IsSuccess || confirmationTokenResult.Data == null)
            {
                return new Result<int>(false, "Invalid or expired token", -1, 401);
            }

            if (confirmationTokenResult.Data.IsUsed || confirmationTokenResult.Data.ExpireAt < DateTime.UtcNow)
            {
                return new Result<int>(false, "Invalid or expired token", -1, 401);
            }

            return new Result<int>(true, "valid token", confirmationTokenResult.Data.UserId, 200);
        }

        public async Task<Result<int>> ValidateCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return new Result<int>(false, "code is required", -1, 400);
            }

            var confirmationTokenResult = await _confirmationTokenService.GetByCodeAsync(code);


            if (!confirmationTokenResult.IsSuccess || confirmationTokenResult.Data == null)
            {
                return new Result<int>(false, "Invalid or expired code", -1, 401);
            }

            if (confirmationTokenResult.Data.IsUsed || confirmationTokenResult.Data.ExpireAt < DateTime.UtcNow)
            {
                return new Result<int>(false, "Invalid or expired code", -1, 401);
            }

            return new Result<int>(true, "valid code", confirmationTokenResult.Data.UserId, 200);
        }
    }
}
