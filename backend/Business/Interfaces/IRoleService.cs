using Jannara_Ecommerce.DTOs.Role;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IRoleService
    {
        public Task<Result<RoleDTO>> FindAsync(int id);
        public Task<Result<RoleDTO>> AddNewAsync(RoleDTO newRole);
        public Task<Result<bool>> UpdateAsync(int id, RoleDTO updatedRole);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
