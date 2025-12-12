using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<Result<RefreshTokenDTO>> AddNewAsync(int userId, string token, DateTime expires);
        Task<Result<RefreshTokenDTO>> GetByToken(string token);
    }
}
