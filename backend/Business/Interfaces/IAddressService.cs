using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IAddressService
    {
        public Task<Result<AddressDTO>> FindAsync(int id);
        public Task<Result<AddressDTO>> AddNewAsync(AddressDTO newAddress);
        public Task<Result<bool>> UpdateAsync(int id, AddressDTO updatedAddress);
        public Task<Result<bool>> DeleteAsync(int id);

    }
}
