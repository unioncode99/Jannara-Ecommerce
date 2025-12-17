using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IConfirmationService
    {
        public Task<Result<bool>> SendAccountConfirmationAsync(UserPublicDTO userInfo);
        public Task<Result<bool>> SendForgetPasswordConfirmationAsync(UserDTO userInfo);
        public Task<Result<int>> ValidateTokenAsync(string token);
        public Task<Result<int>> ValidateCodeAsync(string code);
    }
}
