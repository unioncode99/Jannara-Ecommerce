using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IProductRepository
    {
        public Task<Result<PagedResponseDTO<ProductResponseDTO>>> GetAllAsync(int pageNumber = 1, int pageSize = 20, int customerId = -1);
    }
}
