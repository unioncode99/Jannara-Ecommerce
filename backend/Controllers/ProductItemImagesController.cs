using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.ProductItemImage;
using Jannara_Ecommerce.DTOs.Variation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    [Route("api/product-item-images")]
    [ApiController]
    public class ProductItemImagesController : ControllerBase
    {
        

        private readonly IProductItemImageService _service;
        public ProductItemImagesController(IProductItemImageService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> AddImages([FromForm] ProductItemImageCreateOneDTO request)
        {
            var result = await _service.AddNewImagesAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> SetPrimary(int id)
        {
            var result = await _service.SetPrimaryAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> SetImage(int id)
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
