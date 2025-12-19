using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IProductService
    {
        public Task<Result<PagedResponseDTO<ProductResponseDTO>>> GetAllAsync(int pageNumber, int pageSize, int customerId = -1);
    }
}
