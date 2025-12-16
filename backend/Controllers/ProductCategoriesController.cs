using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/product-categories")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;
        public ProductCategoriesController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ProductCategoryDTO>> GetAll()
        {
            var result = await _productCategoryService.GetAllAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }
    }
}
