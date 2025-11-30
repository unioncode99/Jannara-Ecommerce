using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IRoleRepository
    {
        public Task<Result<RoleDTO>> AddNewAsync(RoleDTO newRole);
        public Task<Result<bool>> UpdateAsync(int id, RoleDTO updatedRole);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<RoleDTO>> GetById(int id);
    }
}
