using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.CustomerWishlist;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/customer-wish-list")]
    [ApiController]
    public class CustomerWishlistController : ControllerBase
    {
        private readonly ICustomerWishlistService _customerWishlistService;
        public CustomerWishlistController(ICustomerWishlistService customerWishlistService)
        {
            _customerWishlistService = customerWishlistService;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerWishlistDTO>> AddCustomerWishlist(CustomerWishlistCreateDTO customerWishlist)
        {
            var result = await _customerWishlistService.AddNewAsync(customerWishlist); 
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteCustomerWishlist(CustomerWishlistCreateDTO customerWishlist)
        {
            var result = await _customerWishlistService.DeleteAsync(customerWishlist);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }
    }
}
