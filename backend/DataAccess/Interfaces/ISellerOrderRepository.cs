using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.SellerOrder;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface ISellerOrderRepository
    {
        Task<Result<PagedResponseDTO<SellerOrderResponseDTO>>> GetSellerOrdersAsync(SellerOrderFilterDTO filter);
        Task<Result<bool>> UpdateOrderStatusAsync(ChangeSellerOrderStatusRequest request);
    }
}
