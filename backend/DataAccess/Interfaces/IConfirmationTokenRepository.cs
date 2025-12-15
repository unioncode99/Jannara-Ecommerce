using Jannara_Ecommerce.DTOs.Token;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IConfirmationTokenRepository
    {
        public Task<Result<int>> AddNewAsync(ConfirmationTokenDTO passwordResetTokenDTO);

        public Task<Result<ConfirmationTokenDTO>> GetByTokenAsync(string token);
        public Task<Result<ConfirmationTokenDTO>> GetByCodeAsync(string code);
        public Task<Result<bool>> MarkAsUsedAsync(int user_id, SqlConnection conn, SqlTransaction transaction);
        public Task<Result<bool>> MarkAsUsedAsync(int user_id);
    }
}
