using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.Order;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.DTOs.SellerOrder;
using Jannara_Ecommerce.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    [Route("api/seller-orders")]
    [ApiController]
    public class SellerOrdersController : ControllerBase
    {

        private readonly ISellerOrderService _service;
        public SellerOrdersController(ISellerOrderService service)
        {
            _service = service;
        }


        [HttpGet(Name = "GetSellerOrders")]
        public async Task<ActionResult<ProductDetailDTO>> GetSellerOrders([FromQuery] SellerOrderFilterDTO filter)
        {
            if (filter.PageNumber <= 0 || filter.PageSize <= 0)
            {
                return BadRequest(new ResponseMessage("invalid_pagination_parameters"));
            }

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized("User not authenticated.");
            }

            int.TryParse(userIdClaim.Value, out int userId);

            filter.UserId = userId;

            var result = await _service.GetSellerOrdersAsync(filter);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] ChangeSellerOrderStatusRequest request)
        {
            var result = await _service.UpdateOrderStatusAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

    }
}
