using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.Business.Services;
using Jannara_Ecommerce.DTOs.Brand;
using Jannara_Ecommerce.DTOs.ProductCategory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    [Route("api/brands")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _service;
        public BrandsController(IBrandService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }


        [HttpPost]
        public async Task<ActionResult<BrandDTO>> AddBrand([FromBody] BrandCreateDTO newBrand)
        {
            var result = await _service.AddNewAsync(newBrand);
            if (result.IsSuccess)
            {
                //return CreatedAtRoute("GetSellerByID", new { id = result.Data.Id }, result.Data);
                //return CreatedAtRoute("GetSellerByID", new { id = result.Data.Id }, result);
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BrandDTO>> UpdateBrand(int id, [FromBody] BrandUpdateDTO updatedBrand)
        {

            var result = await _service.UpdateAsync(id, updatedBrand);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
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
