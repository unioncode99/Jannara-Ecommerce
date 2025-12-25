using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.DTOs.CustomerWishlist;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface ICustomerWishlistService
    {
        public Task<Result<CustomerWishlistDTO>> AddNewAsync(CustomerWishlistCreateDTO newCustomerWishlist);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<bool>> DeleteAsync(CustomerWishlistCreateDTO customerWishlist);
    }
}
