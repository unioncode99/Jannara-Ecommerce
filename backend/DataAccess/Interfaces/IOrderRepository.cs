using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Order;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IOrderRepository
    {
        Task<Result<OrderDTO>> CreateAsync(OrderCreateDTO orderCreateRequest);
        Task<Result<OrderDTO>> ConfirmPaymentAsync(int? orderId, string? paymentIntentId, int paymentMethodId);
        Task<Result<OrderDetailsDTO>> GetByPublicIdAsync(string publicId);
        Task<Result<PagedResponseDTO<OrderDetailsDTO>>> GetCustomerOrdersAsync(FilterCustomerOrderDTO filterCustomerOrderDTO);
        Task<Result<bool>> CancelOrderAsync(OrderCancelRequestDTO orderCancelRequestDTO);

    }
}
