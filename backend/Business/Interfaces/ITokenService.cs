using Jannara_Ecommerce.DTOs.Token;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface ITokenService
    {
        AccessTokenDTO GenerateAccessToken(UserDTO user);
        string GenerateRefreshToken();

        public string GenerateResetToken(int length = 32);

    }
}
