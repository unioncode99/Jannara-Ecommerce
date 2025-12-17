using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Token;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Services
{
    public class ConfirmationTokenService : IConfirmationTokenService
    {
        private readonly IConfirmationTokenRepository _repo;
        public ConfirmationTokenService(IConfirmationTokenRepository repo) 
        {
            _repo = repo;
        }
        public async Task<Result<int>> AddNewAsync(ConfirmationTokenDTO passwordResetTokenDTO)
        {
            return await _repo.AddNewAsync(passwordResetTokenDTO);
        }

        public async Task<Result<ConfirmationTokenDTO>> GetByTokenAsync(string token)
        {
            return await _repo.GetByTokenAsync(token);
        }

        public async Task<Result<ConfirmationTokenDTO>> GetByCodeAsync(string code)
        {
            return await _repo.GetByCodeAsync(code);
        }

        public async Task<Result<bool>> MarkAsUsedAsync(int id, SqlConnection conn, SqlTransaction transaction)
        {
            return await _repo.MarkAsUsedAsync(id, conn, transaction);
        }

        public async Task<Result<bool>> MarkAsUsedAsync(int userId)
        {
            return await _repo.MarkAsUsedAsync(userId);
        }

        public async Task<Result<ConfirmationTokenDTO>> GetByEmailAsync(string email)
        {
            return await _repo.GetByEmailAsync(email);
        }
    }
}
