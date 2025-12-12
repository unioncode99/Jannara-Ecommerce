using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IAddressRepository
    {
        public Task<Result<AddressDTO>> AddNewAsync(AddressDTO newAddress);
        public Task<Result<bool>> UpdateAsync(int id, AddressDTO updatedAddress);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<AddressDTO>> GetByIdAsync(int id);
        public Task<Result<IEnumerable<AddressDTO>>> GetAllAsync(int personId);
    }
}
