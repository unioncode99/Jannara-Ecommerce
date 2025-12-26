using Jannara_Ecommerce.DTOs.Address;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IAddressRepository
    {
        public Task<Result<AddressDTO>> AddNewAsync(AddressCreateDTO addressCreateDTO);
        public Task<Result<bool>> UpdateAsync(int id, AddressUpdateDTO addressUpdateDTO);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<AddressDTO>> GetByIdAsync(int id);
        public Task<Result<AddressResponseDTO>> GetAllAsync(int personId);
    }
}
