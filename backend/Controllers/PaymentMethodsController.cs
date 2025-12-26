using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.Business.Services;
using Jannara_Ecommerce.DTOs.PaymentMethod;
using Jannara_Ecommerce.DTOs.ShippingMethod;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    [Route("api/payment-methods")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodsController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentMethodDTO>>> GetAllActiveAsync()
        {
            var result = await _paymentMethodService.GetAllActiveAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

    }
}
