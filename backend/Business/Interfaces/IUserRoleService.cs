using Jannara_Ecommerce.DTOs.UserRole;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IUserRoleService
    {
        public Task<Result<UserRoleDTO>> FindAsync(int id);
        public Task<Result<UserRoleDTO>> AddNewAsync(int roleId, int userId, bool isActive, SqlConnection connection, SqlTransaction transaction);
        public Task<Result<bool>> UpdateAsync(int id, UserRoleUpdateDTO updatedUserRole);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
