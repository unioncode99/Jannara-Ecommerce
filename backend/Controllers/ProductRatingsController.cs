using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.ProductCategory;
using Jannara_Ecommerce.DTOs.ProductRating;
using Jannara_Ecommerce.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    [Route("api/product-ratings")]
    [ApiController]
    public class ProductRatingsController : ControllerBase
    {
        private readonly IProductRatingService _service;
        public ProductRatingsController(IProductRatingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponseDTO<ProductRatingDetailsDTO>>> GetAll([FromQuery] ProductRatingFilterDTO filter)
        {
            var result = await _service.GetAllAsync(filter);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }
        [HttpPost]
        public async Task<ActionResult<ProductRatingDTO>> AddProductRating([FromBody] ProductRatingCreateDTO newProductRating)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized("User not authenticated.");
            }

            int.TryParse(userIdClaim.Value, out int userId);
            newProductRating.UserId = userId;

            var result = await _service.AddNewAsync(newProductRating);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductRatingDTO>> UpdateProductRating(int id, [FromBody] ProductRatingUpdateDTO updateProductRating)
        {

            var result = await _service.UpdateAsync(id, updateProductRating);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductRating(int id)
        {
            if (id <= 0)
            {
                return BadRequest("invalid_data");
            }
            var result = await _service.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }
    }
}
