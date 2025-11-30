using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IUserRoleRepository
    {
        public Task<Result<UserRoleDTO>> AddNewAsync(UserRoleDTO newUserRole);
        public Task<Result<bool>> UpdateAsync(int id, UserRoleDTO updatedUserRole);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<UserRoleDTO>> GetByIdAsync(int id);
        public Task<Result<IEnumerable<UserRoleDTO>>> GetAllAsync(int user_id);
    }
}
