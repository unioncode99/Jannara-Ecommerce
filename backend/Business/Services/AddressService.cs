using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<Result<AddressDTO>> AddNewAsync(AddressDTO newAddress)
        {
            return await _addressRepository.AddNewAsync(newAddress);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _addressRepository.DeleteAsync(id);
        }

        public async Task<Result<AddressDTO>> FindAsync(int id)
        {
            return await _addressRepository.GetByIdAsync(id);
        }

        public async Task<Result<bool>> UpdateAsync(int id, AddressDTO updatedAddress)
        {
            return await _addressRepository.UpdateAsync(id, updatedAddress);
        }
    }
}
