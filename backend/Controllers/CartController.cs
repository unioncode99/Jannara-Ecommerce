using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.CartItem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICartItemService _cartItemService;

        public CartController(ICartItemService cartItemService, ICartService cartService)
        {
            _cartItemService = cartItemService;
            _cartService = cartService;
        }

        [HttpGet()]
        public async Task<ActionResult> GetActiveCart(int customerId)
        {
            var result = await _cartService.GetActiveCartAsync(customerId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        //[HttpPost("add-or-update")]
        [HttpPost]
        public async Task<ActionResult> AddToCart([FromBody] CartItemRequestDTO cartItemRequest)
        {
            var result = await _cartItemService.AddOrUpdateAsync(cartItemRequest);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpDelete("item/{cartItemId}")]
        public async Task<ActionResult> RemoveItem(int cartItemId)
        {
            var result = await _cartItemService.DeleteAsync(cartItemId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPost("clear/{cartId}")]
        public async Task<ActionResult> ClearCart(int cartId)
        {
            var result = await _cartService.ClearCartAsync(cartId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }
    }
}
