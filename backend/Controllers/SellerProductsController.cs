using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.DTOs.SellerProduct;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    [Route("api/seller-products")]
    [ApiController]
    public class SellerProductsController : ControllerBase
    {
        private readonly ISellerProductService _sellerProductService;

        public SellerProductsController(ISellerProductService sellerProductService)
        {
            _sellerProductService = sellerProductService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponseDTO<SellerProductResponseDTO>>> GetAllProducts([FromQuery] SellerProductFilterDTO filter)
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

            var result = await _sellerProductService.GetAllAsync(filter);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

    }
}
