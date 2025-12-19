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

        public async Task<ActionResult<PagedResponseDTO<ProductResponseDTO>>> GetAll([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] int customerId)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest(new ResponseMessage("invalid_pagination_parameters"));
            }
            var result = await _productService.GetAllAsync(pageNumber, pageSize, customerId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }
    }
}
