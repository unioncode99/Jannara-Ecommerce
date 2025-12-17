using Jannara_Ecommerce.DTOs.Authentication;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Utilities;
using Jannara_Ecommerce.Utilities.WrapperClasses;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IUserConfirmationService
    {

        public Task<Result<UserDTO>> GetUserForConfirmationAsync(int id);
        public Task<Result<UserDTO>> GetUserForConfirmationAsync(string email);
        //public Task<Result<bool>> MarkUserAsConfirmedAsync(string email);
    }
}
