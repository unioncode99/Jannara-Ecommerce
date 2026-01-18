using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Order;
using Jannara_Ecommerce.DTOs.SellerOrder;
using Jannara_Ecommerce.Utilities;
using Stripe;

namespace Jannara_Ecommerce.Business.Services
{
    public class SellerOrderService : ISellerOrderService
    {

        private readonly ISellerOrderRepository _sellerOrderRepository;
        private readonly IConfiguration _config;
        public SellerOrderService(ISellerOrderRepository sellerOrderRepository)
        {
            _sellerOrderRepository = sellerOrderRepository;
        }

        public async Task<Result<PagedResponseDTO<SellerOrderResponseDTO>>> GetSellerOrdersAsync(SellerOrderFilterDTO filter)
        {
            return await _sellerOrderRepository.GetSellerOrdersAsync(filter);   
        }

        public async Task<Result<bool>> UpdateOrderStatusAsync(ChangeSellerOrderStatusRequest request)
        {
            if (request.OrderId == null && string.IsNullOrEmpty(request.PublicId))
            {
                return new Result<bool>(false, "Order ID or Public ID is required", false, 400);
            }

            return await _sellerOrderRepository.UpdateOrderStatusAsync(request);
        }
    }
}
