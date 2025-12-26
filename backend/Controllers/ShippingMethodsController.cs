using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.Business.Services;
using Jannara_Ecommerce.DTOs.Address;
using Jannara_Ecommerce.DTOs.ShippingMethod;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    [Route("api/shipping-methods")]
    [ApiController]
    public class ShippingMethodsController : ControllerBase
    {
        private readonly IShippingMethodService _shippingMethodService;

        public ShippingMethodsController(IShippingMethodService shippingMethodService)
        {
            _shippingMethodService = shippingMethodService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShippingMethodDTO>>> GetAllActiveAsync()
        {
            var result = await _shippingMethodService.GetAllActiveAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }
    }
}
