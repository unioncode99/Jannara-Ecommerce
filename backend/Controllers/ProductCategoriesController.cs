using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.ProductCategory;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

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
        public async Task<ActionResult<IEnumerable<ProductCategoryDTO>>> GetAll()
        {
            var result = await _productCategoryService.GetAllAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }
        [HttpPost]
        public async Task<ActionResult<ProductCategoryDTO>> AddProductCategory([FromBody] ProductCategoryCreateDTO newProductCategory)
        {
            var result = await _productCategoryService.AddNewAsync(newProductCategory);
            if (result.IsSuccess)
            {
                //return CreatedAtRoute("GetSellerByID", new { id = result.Data.Id }, result.Data);
                //return CreatedAtRoute("GetSellerByID", new { id = result.Data.Id }, result);
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductCategoryDTO>> UpdateProductCategory(int id, [FromBody] ProductCategoryUpdateDTO updateProductCategory)
        {

            var result = await _productCategoryService.UpdateAsync(id, updateProductCategory);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductCategory(int id)
        {
            if (id <= 0)
            {
                return BadRequest("invalid_data");
            }
            var result = await _productCategoryService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

    }
}
