using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Token;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Services
{
    public class ConfirmationTokenService : IConfirmationTokenService
    {
        IConfirmationTokenRepository _confirmationTokenRepository;
        public ConfirmationTokenService(IConfirmationTokenRepository ConfirmationTokenRepository) 
        {
            _confirmationTokenRepository = ConfirmationTokenRepository;
        }
        public async Task<Result<int>> AddNewAsync(ConfirmationTokenDTO passwordResetTokenDTO)
        {
            return await _confirmationTokenRepository.AddNewAsync(passwordResetTokenDTO);
        }

        public async Task<Result<ConfirmationTokenDTO>> GetByTokenAsync(string token)
        {
            return await _confirmationTokenRepository.GetByTokenAsync(token);
        }

        public async Task<Result<ConfirmationTokenDTO>> GetByCodeAsync(string code)
        {
            return await _confirmationTokenRepository.GetByCodeAsync(code);
        }

        public async Task<Result<bool>> MarkAsUsedAsync(int id, SqlConnection conn, SqlTransaction transaction)
        {
            return await _confirmationTokenRepository.MarkAsUsedAsync(id, conn, transaction);
        }

        public async Task<Result<bool>> MarkAsUsedAsync(int id)
        {
            return await _confirmationTokenRepository.MarkAsUsedAsync(id);
        }

    }
}
