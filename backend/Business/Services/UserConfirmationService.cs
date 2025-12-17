using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Services
{
    public class UserConfirmationService : IUserConfirmationService
    {
        private readonly IUserRepository _repo;
        public UserConfirmationService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<UserDTO>> GetUserForConfirmationAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Result<UserDTO>> GetUserForConfirmationAsync(string email)
        {
            return await _repo.GetByEmailAsync(email);
        }
    }
}
