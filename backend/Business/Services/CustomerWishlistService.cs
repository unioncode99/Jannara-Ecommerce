using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.CustomerWishlist;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Services
{
    public class CustomerWishlistService : ICustomerWishlistService
    {
        private readonly ICustomerWishlistRepository _customerWishlistRepository;

        public CustomerWishlistService(ICustomerWishlistRepository customerWishlistRepository)
        {
            _customerWishlistRepository = customerWishlistRepository;
        }

        public async Task<Result<CustomerWishlistDTO>> AddNewAsync(CustomerWishlistCreateDTO newCustomerWishlist)
        {
            return await _customerWishlistRepository.AddNewAsync(newCustomerWishlist);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _customerWishlistRepository.DeleteAsync(id);
        }

        public async Task<Result<bool>> DeleteAsync(CustomerWishlistCreateDTO customerWishlist)
        {
            return await _customerWishlistRepository.DeleteAsync(customerWishlist);
        }
    }
}
