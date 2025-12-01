using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.Business.Services;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Enums;
using Jannara_Ecommerce.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Jannara_Ecommerce.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;
        public CustomersController(ICustomerService service)
        {
            _service = service;
        }
        
        [HttpGet("{id}", Name = "GetCustomerByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CustomerDTO>> GetCustomerByID(int id)
        {
            Result<CustomerDTO> result = await _service.FindAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return result.ErrorCode == 400 ? BadRequest(result.Message) : NotFound(result.Message);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CustomerDTO>> AddCustomer(RegisteredCustomerDTO registeredCustomerDTO)
        {
            Result<CustomerDTO> result = await _service.RegisterAsync(registeredCustomerDTO);
           
            if (result.IsSuccess)
            {
                return CreatedAtRoute("GetCustomerByID", new { id = result.Data.Id }, result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            Result<bool> result = await _service.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

    }
}
