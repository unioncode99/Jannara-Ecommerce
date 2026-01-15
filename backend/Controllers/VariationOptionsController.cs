using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.DTOs.Variation;
using Jannara_Ecommerce.DTOs.VariationOption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    [Route("api/variation-options")]
    [ApiController]
    public class VariationOptionsController : ControllerBase
    {

        private readonly IVariationOptionService _service;
        public VariationOptionsController(IVariationOptionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> AddVariationOption(VariationOptionCreateOneDTO request)
        {
            var result = await _service.AddNewAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateVariationOption(int id, [FromBody] VariationOptionUpdateDTO request)
        {
            var result = await _service.UpdateAsync(id, request);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteVariationOption(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

    }
}
