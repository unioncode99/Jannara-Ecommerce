using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.Order;
using Jannara_Ecommerce.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Jannara_Ecommerce.Controllers
{
    [Route("api/webhook")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly IConfiguration _config;

        public WebhookController(IOrderService service, IConfiguration config)
        {
            _service = service;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Handle()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();

            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _config["Stripe:WebhookSecret"]
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Webhook signature verification failed: {ex}");
                return BadRequest();
            }

            // Handle ONLY what you care about
            //if (stripeEvent.Type != Events.PaymentIntentSucceeded)
            if (stripeEvent.Type != "payment_intent.succeeded")
            {
                Console.WriteLine("Not Success {0}", stripeEvent.Type);
                return Ok(); // ignore safely
            }

            var intent = stripeEvent.Data.Object as PaymentIntent;
            if (intent == null)
            {
                Console.WriteLine("PaymentIntent was null");
                return Ok();
            }

            Result<OrderDTO> confirmPaymentResult = await _service.ConfirmPaymentAsync(null, intent.Id, 2);

            //if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            //if (stripeEvent.Type == "payment_intent.succeeded")
            //{
            //    var intent = stripeEvent.Data.Object as PaymentIntent;
            //    confirmPaymentResult = await _service.ConfirmPaymentAsync(intent.Id);
            //}

            if (confirmPaymentResult != null && confirmPaymentResult.IsSuccess)
            {
                Console.WriteLine($"Payment confirmation success for intent {intent.Id}");
                return Ok();
            }

            Console.WriteLine($"Payment confirmed for intent {intent.Id}");
            Console.WriteLine("Success {0}", stripeEvent.Type);
            return Ok();

        }
    }
}
