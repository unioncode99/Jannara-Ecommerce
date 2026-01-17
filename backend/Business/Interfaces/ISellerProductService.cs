using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.SellerProduct;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface ISellerProductService
    {
        Task<Result<PagedResponseDTO<SellerProductResponseDTO>>> GetAllAsync(SellerProductFilterDTO filter);
    }
}
