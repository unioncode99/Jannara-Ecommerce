using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        public UserRoleService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<Result<UserRoleDTO>> AddNewAsync(int roleId, int userId, bool isActive, SqlConnection connection, SqlTransaction transaction)
        {
            return await _userRoleRepository.AddNewAsync(roleId, userId, isActive, connection, transaction);
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
