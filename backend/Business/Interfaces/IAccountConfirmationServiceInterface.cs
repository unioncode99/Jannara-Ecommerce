using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IAccountConfirmationServiceInterface
    {
        public Task<Result<bool>> SendAccountConfirmationAsync(UserPublicDTO userInfo, SqlConnection conn, SqlTransaction transaction);
    }
}
