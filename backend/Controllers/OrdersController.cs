using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.Order;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;
        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder(OrderCreateDTO orderCreateRequest)
        {
            var result = await _service.PlaceOrderAsync(orderCreateRequest);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpGet("{publicId}", Name = "GetOrderByPublicId")]
        public async Task<ActionResult<ProductDetailDTO>> GetOrderByPublicId(string publicId)
        {
            var result = await _service.GetByPublicIdAsync(publicId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpGet(Name = "GetCustomerOrders")]
        public async Task<ActionResult<ProductDetailDTO>> GetCustomerOrders([FromQuery] FilterCustomerOrderDTO filterCustomerOrderDTO)
        {
            if (filterCustomerOrderDTO.PageNumber <= 0 || filterCustomerOrderDTO.PageSize <= 0)
            {
                return BadRequest(new ResponseMessage("invalid_pagination_parameters"));
            }

            var result = await _service.GetCustomerOrdersAsync(filterCustomerOrderDTO);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPatch("cancel")]
        public async Task<IActionResult> CancelOrder([FromBody] OrderCancelRequestDTO orderCancelRequestDTO)
        {
            var result = await _service.CancelOrderAsync(orderCancelRequestDTO);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }
    }
}
