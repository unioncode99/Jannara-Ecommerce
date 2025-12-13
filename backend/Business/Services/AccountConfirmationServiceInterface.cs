using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Token;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Jannara_Ecommerce.Business.Services
{
    public class AccountConfirmationServiceInterface : IAccountConfirmationServiceInterface
    {
        private readonly IUserConfirmationService _userConfirmationService;
        private readonly IConfirmationTokenServiceInterface _confirmationTokenService;
        private readonly ITokenService _tokenService;
        private readonly ICodeService _codeService;
        private readonly IConfiguration _configuration;
        private readonly IEmailSenderService _emailSenderService;
        private readonly ILogger<IAccountConfirmationServiceInterface> _logger;
        public AccountConfirmationServiceInterface(IUserConfirmationService userConfirmationService, IConfirmationTokenServiceInterface confirmationTokenService, 
            ITokenService tokenService, ICodeService codeService, 
            IConfiguration configuration, IEmailSenderService emailSenderService,
            ILogger<IAccountConfirmationServiceInterface> logger)
        {
            _userConfirmationService = userConfirmationService;
            _confirmationTokenService = confirmationTokenService;
            _tokenService = tokenService;
            _codeService = codeService;
            _configuration = configuration;
            _emailSenderService = emailSenderService;
            _logger = logger;
        }

        public async Task<Result<bool>> SendAccountConfirmationAsync(UserPublicDTO userInfo, SqlConnection conn, SqlTransaction transaction)
        {
            string token = _tokenService.GenerateResetToken();
            string code = _codeService.GenerateCode(4);

            ConfirmationTokenDTO accountConfirmationDTO = new ConfirmationTokenDTO(0, userInfo.Id, token, code, DateTime.Now.AddMinutes(15), false);
            Result<int> saveTokenResult = await _confirmationTokenService.AddNewAsync(accountConfirmationDTO, conn, transaction);
            if (!saveTokenResult.IsSuccess)
            {
                return new Result<bool>(false, "internal_server_error", false, 500);
            }

            string confirmUrl = $"{_configuration.GetValue<string>("EMAIL_CONFIGURATION:FRONTEND_DOMAIN")}/confirm-account/{token}";
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
      <div class='code'>{code}</div>
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

            try
            {
                await _emailSenderService.SendEmailAsync(userInfo.Email, "Account Confirmation", body);
                return new Result<bool>(true, "account_confirmation_link_sent", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Result<bool>(false, "internal_server_error", false, 500);
            }

        }
    }
}
