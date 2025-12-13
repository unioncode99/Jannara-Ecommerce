using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Token;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Services
{
    public class ConfirmationTokenService : IConfirmationTokenServiceInterface
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
    }
}
