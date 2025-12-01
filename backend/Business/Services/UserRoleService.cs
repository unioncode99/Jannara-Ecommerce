using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        public UserRoleService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<Result<UserRoleDTO>> AddNewAsync(UserRoleDTO newUserRole)
        {
            return await _userRoleRepository.AddNewAsync(newUserRole);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _userRoleRepository.DeleteAsync(id);
        }

        public async Task<Result<UserRoleDTO>> FindAsync(int id)
        {
            return await _userRoleRepository.GetByIdAsync(id);
        }

        public async Task<Result<bool>> UpdateAsync(int id, UserRoleDTO updatedUserRole)
        {
            return await _userRoleRepository.UpdateAsync(id, updatedUserRole);
        }
    }
}
