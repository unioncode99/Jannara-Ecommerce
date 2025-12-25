using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet("{publicId}", Name = "GetProductByPublicId"), Route("details")]
        public async Task<ActionResult<ProductDetailDTO>> GetProductByPublicId([FromQuery] Guid publicId, [FromQuery] int? customerId)
        {
            var result = await _productService.FindAsync(publicId, customerId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponseDTO<ProductResponseDTO>>> GetAllProducts([FromQuery] FilterProductDTO filter)
        {
            if (filter.PageNumber <= 0 || filter.PageSize <= 0)
            {
                return BadRequest(new ResponseMessage("invalid_pagination_parameters"));
            }
            var result = await _productService.GetAllAsync(filter);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

    
    }
}
