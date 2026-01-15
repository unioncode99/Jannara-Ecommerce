using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.DTOs.Variation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    [Route("api/variations")]
    [ApiController]
    public class VariationsController : ControllerBase
    {
        private readonly IVariationService _service;
        public VariationsController(IVariationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<UserPublicDTO>> AddVariation(VariationCreateOneDTO request)
        {
            var result = await _service.AddNewAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateVariation(int id, [FromBody] VariationUpdateDTO request)
        {
            var result = await _service.UpdateAsync(id, request);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteVariation(int id)
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
