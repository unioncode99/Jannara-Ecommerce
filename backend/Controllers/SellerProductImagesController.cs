using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.ProductItemImage;
using Jannara_Ecommerce.DTOs.SellerProductImage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    [Route("api/seller-product-images")]
    [ApiController]
    public class SellerProductImagesController : ControllerBase
    {

        private readonly ISellerProductImageService _service;
        public SellerProductImagesController(ISellerProductImageService service)
        {
            _service = service;
        }


        [HttpPost]
        public async Task<ActionResult> AddImages([FromForm] SellerProductImageCreateOneDTO request)
        {
            var result = await _service.AddNewImagesAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteImage(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

    }
}
