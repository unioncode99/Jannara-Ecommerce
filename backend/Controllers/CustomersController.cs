using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Role;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;
        public CustomersController(ICustomerService service)
        {
            _service = service;
        }
        
        [HttpGet("{id:int}", Name = "GetCustomerByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CustomerDTO>> GetCustomerById(int id)
        {
            var result = await _service.FindAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CustomerDTO>> AddCustomer(CustomerCreateRequestDTO request)
        {

            var result = await _service.CreateAsync(request);
           
            if (result.IsSuccess)
            {
                //return CreatedAtRoute("GetCustomerByID", new { id = result.Data.Id }, result.Data);
                return CreatedAtRoute("GetCustomerByID", new { id = result.Data.Id }, result);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPost("become-seller")]
        public async Task<ActionResult<RoleDTO>> BecomeASeller([FromBody] BecomeSellerDTO becomeSellerDTO)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized("User not authenticated.");
            }

            int.TryParse(userIdClaim.Value, out int userId);

            becomeSellerDTO.UserId = userId;

            var result = await _service.BecomeASeller(becomeSellerDTO);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResponseDTO<CustomerDTO>>> GetAll([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            if (pageNumber == 0 || pageSize == 0)
            {
                return BadRequest(new ResponseMessage("invalid_pagination_parameters"));
            }
            var result = await _service.GetAllAsync(pageNumber, pageSize);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

    }
}
