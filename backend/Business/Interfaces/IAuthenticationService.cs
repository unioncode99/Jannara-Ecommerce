using Jannara_Ecommerce.DTOs.Authentication;
using Jannara_Ecommerce.Utilities;
using Jannara_Ecommerce.Utilities.WrapperClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<Result<LoginResult>> LogInAsync(LoginDTO request);

        public Task<Result<bool>> ForgetPasswordAsync(string email);

        public Task<Result<bool>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
        public Task<Result<VerifyCodeResposeDTO>> VerifyCodeAsync(string Code);
        public Task<Result<LoginResult>> ConfirmAccountAsync(string token);
        public Task<Result<bool>> ResendAccountConfirmationAsync(string email);
    }
}
