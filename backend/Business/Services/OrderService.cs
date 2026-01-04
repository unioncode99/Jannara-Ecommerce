using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Order;
using Jannara_Ecommerce.Utilities;
using Stripe;

namespace Jannara_Ecommerce.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IConfiguration _config;
        public OrderService(IOrderRepository orderRepository, IConfiguration config)
        {
            _orderRepository = orderRepository;
            _config = config;
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        }

        public async Task<Result<bool>> CancelOrderAsync(OrderCancelRequestDTO orderCancelRequestDTO)
        {
            if (orderCancelRequestDTO.OrderId == null && string.IsNullOrEmpty(orderCancelRequestDTO.PublicId))
            {
                return new Result<bool>(false, "Order ID or Public ID is required", false, 400);
            }

            return await _orderRepository.CancelOrderAsync(orderCancelRequestDTO);
        }

        public async Task<Result<OrderDTO>> ConfirmPaymentAsync(int? orderId, string? paymentIntentId, int paymentMethodId)
        {
            return await _orderRepository.ConfirmPaymentAsync(orderId, paymentIntentId, paymentMethodId);
        }

        public async Task<Result<OrderDTO>> CreateAsync(OrderCreateDTO orderCreateRequest)
        {
            return await _orderRepository.CreateAsync(orderCreateRequest);  
        }

        public async Task<Result<OrderDetailsDTO>> GetByPublicIdAsync(string publicId)
        {
            return await _orderRepository.GetByPublicIdAsync(publicId);
        }

        public async Task<Result<PagedResponseDTO<OrderDetailsDTO>>> GetCustomerOrdersAsync(FilterCustomerOrderDTO filterCustomerOrderDTO)
        {
            return await _orderRepository.GetCustomerOrdersAsync(filterCustomerOrderDTO);
        }

        public async Task<Result<PlaceOrderResponseDTO>> PlaceOrderAsync(OrderCreateDTO orderCreateRequest)
        {
            orderCreateRequest.TaxRate =
            decimal.TryParse(_config["TaxRate"], out var tax)
            ? tax
            : null;

            if (orderCreateRequest.PaymentMethodId == 1)
            {
                orderCreateRequest.PayNow = false;
                var orderResult = await CreateAsync(orderCreateRequest);

                if (orderResult == null || !orderResult.IsSuccess || orderResult.Data == null)
                {
                    return new Result<PlaceOrderResponseDTO>(false, "order fail", null, 500);
                }
                return new Result<PlaceOrderResponseDTO>(true, "order placed", new PlaceOrderResponseDTO(orderResult.Data, string.Empty), 200);
            }

            if (orderCreateRequest == null || orderCreateRequest.GrandTotal <= 0)
            {
                return new Result<PlaceOrderResponseDTO>(false, "invalid data", null, 400);
            }

            var total = orderCreateRequest?.GrandTotal ?? 0;
            PaymentIntent intent;
            try
            {
                intent = await new PaymentIntentService().CreateAsync(
                new PaymentIntentCreateOptions
                {
                    Amount = (long?)(total * 100),
                    Currency = "usd",
                    AutomaticPaymentMethods = new() { Enabled = true }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("PaymentIntentService().CreateAsync error {0}", ex);
                return new Result<PlaceOrderResponseDTO>(false, "payment fail", null);
            }
   

            if (intent == null || intent.ClientSecret == null || intent.Id == null)
            {
                return new Result<PlaceOrderResponseDTO>(false, "payment fail", null);
            }

            orderCreateRequest.PaymentIntentId = intent.Id;
            orderCreateRequest.PayNow = false;
            var order = await CreateAsync(orderCreateRequest);

            if (order == null || !order.IsSuccess || order.Data == null)
            {
                return new Result<PlaceOrderResponseDTO>(false, "order fail", null);
            }
            return new Result<PlaceOrderResponseDTO>(true, "order placed", new PlaceOrderResponseDTO(order.Data, intent.ClientSecret));
        }

    }
}
