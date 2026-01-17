using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.SellerProduct;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface ISellerProductRepository
    {
        Task<Result<PagedResponseDTO<SellerProductResponseDTO>>> GetAllAsync(SellerProductFilterDTO filter);
    }
}
