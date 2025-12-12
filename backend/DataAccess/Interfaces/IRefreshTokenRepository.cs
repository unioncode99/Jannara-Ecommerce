using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<Result<RefreshTokenDTO>> AddNewAsync(int userId, string token, DateTime expires);
        Task<Result<RefreshTokenDTO>> GetInfoByTokenAsync(string token);
    }
}
