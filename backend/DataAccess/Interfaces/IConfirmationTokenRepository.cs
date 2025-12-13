using Jannara_Ecommerce.DTOs.Token;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IConfirmationTokenRepository
    {
        public Task<Result<int>> AddNewAsync(ConfirmationTokenDTO passwordResetTokenDTO);
    }
}
