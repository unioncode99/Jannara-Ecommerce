using Jannara_Ecommerce.DTOs.Authentication;
using Jannara_Ecommerce.Utilities;
using Jannara_Ecommerce.Utilities.WrapperClasses;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<Result<LoginResult>> LogInAsync(LoginDTO request);
    }
}
