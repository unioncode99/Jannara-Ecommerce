using Jannara_Ecommerce.DTOs.Address;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IAddressService
    {
        public Task<Result<AddressDTO>> FindAsync(int id);
        public Task<Result<AddressDTO>> AddNewAsync(AddressCreateDTO addressCreateDTO);
        public Task<Result<bool>> UpdateAsync(int id, AddressUpdateDTO addressUpdateDTO);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<IEnumerable<AddressDTO>>> GetAllAsync(int personId);

    }
}
