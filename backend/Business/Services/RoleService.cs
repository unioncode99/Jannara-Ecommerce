using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Role;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<Result<RoleDTO>> AddNewAsync(RoleDTO newRole)
        {
            return await _roleRepository.AddNewAsync(newRole);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _roleRepository.DeleteAsync(id);
        }

        public async Task<Result<RoleDTO>> FindAsync(int id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }

        public async Task<Result<bool>> UpdateAsync(int id, RoleDTO updatedRole)
        {
            return await _roleRepository.UpdateAsync(id, updatedRole);  
        }
    }
}
