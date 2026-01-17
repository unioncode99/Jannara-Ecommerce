using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.DTOs.ProductItem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    [Route("api/product-items")]
    [ApiController]
    public class ProductItemsController : ControllerBase
    {
        private readonly IProductItemService _productService;

        public ProductItemsController(IProductItemService productService)
        {
            _productService = productService;
        }

        [HttpGet("dropdown")]
        public async Task<ActionResult<ProductResponseDTO>> GetProductDropdown([FromQuery] ProductItemDropdownRequest filter)
        {
            var result = await _productService.GetProductDropdownAsync(filter);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }
    }
}
