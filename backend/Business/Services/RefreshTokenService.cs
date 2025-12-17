using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _repo;
        public RefreshTokenService(IRefreshTokenRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<RefreshTokenDTO>> AddNewAsync( int userId, string token,  DateTime expires)
        {
            return await _repo.AddNewAsync(userId, token, expires);
        }

        public async Task<Result<RefreshTokenDTO>> GetByToken(string token)
        {
            return await _repo.GetInfoByTokenAsync(token);
        }
    }
}
