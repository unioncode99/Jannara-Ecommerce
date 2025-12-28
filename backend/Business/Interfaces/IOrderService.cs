using Jannara_Ecommerce.DTOs.Order;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IOrderService
    {
        Task<Result<OrderDTO>> CreateAsync(OrderCreateDTO orderCreateRequest);
        Task<Result<PlaceOrderResponseDTO>> PlaceOrderAsync(OrderCreateDTO orderCreateRequest);
        Task<Result<OrderDTO>> ConfirmPaymentAsync(int? orderId, string? paymentIntentId, int paymentMethodId);
        Task<Result<OrderDetailsDTO>> GetByPublicIdAsync(string publicId);
    }
}
