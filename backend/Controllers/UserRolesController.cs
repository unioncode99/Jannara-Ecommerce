using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.DTOs.UserRole;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Jannara_Ecommerce.Controllers
{
    [Route("api/user-roles")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRoleService _service;
        public UserRolesController(IUserRoleService sellerService)
        {
            _service = sellerService;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateUserRole(int id, [FromBody] UserRoleUpdateDTO updatedSellerDTO)
        {

            var result = await _service.UpdateAsync(id, updatedSellerDTO);
            if (result.IsSuccess)
            {
                return Ok(updatedSellerDTO);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }
    }
}
