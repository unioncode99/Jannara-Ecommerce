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

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SellerProductCreateDTO productCreateDTO)
        {
            //Console.WriteLine($"productCreateDTO: {JsonSerializer.Serialize(productCreateDTO)}");
            //Console.WriteLine($"ProductItems: {JsonSerializer.Serialize(productCreateDTO?.ProductItems)}");
            //Console.WriteLine($"Variations: {JsonSerializer.Serialize(productCreateDTO?.Variations)}");
            if (productCreateDTO == null)
            {
                return BadRequest(new { success = false, message = "Product data is required" });
            }

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized("User not authenticated.");
            }

            int.TryParse(userIdClaim.Value, out int userId);

            productCreateDTO.UserId = userId;

            var result = await _sellerProductService.CreateAsync(productCreateDTO);

            if (!result.IsSuccess)
                return StatusCode(result.ErrorCode, new { success = false, message = result.Message });

            return Ok(new { success = true, message = "Product created successfully" });
        }

        [HttpGet("edit/{id}")]
        public async Task<ActionResult<Result<ProductDetailsForAdminDTO>>> GetProductForEdit(int id)
        {
            var result = await _sellerProductService.GetSellerProductForEditAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] SellerProductUpdateDTO productUpdateDTO)
        {
            var result = await _sellerProductService.UpdateAsync(id, productUpdateDTO);

            if (!result.IsSuccess)
            {
                return StatusCode(result.ErrorCode, result.Message);
            }
            return Ok(result.Data);
        }
    }
}
