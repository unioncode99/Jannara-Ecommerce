using Jannara_Ecommerce.DTOs.Token;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IConfirmationTokenServiceInterface
    {
        public Task<Result<int>> AddNewAsync(ConfirmationTokenDTO passwordResetTokenDTO);
    }
}
