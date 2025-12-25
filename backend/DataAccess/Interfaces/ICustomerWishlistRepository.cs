using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.DTOs.CustomerWishlist;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface ICustomerWishlistRepository
    {
        public Task<Result<CustomerWishlistDTO>> AddNewAsync(CustomerWishlistCreateDTO customerWishlist);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<bool>> DeleteAsync(CustomerWishlistCreateDTO customerWishlist);
    }
}
