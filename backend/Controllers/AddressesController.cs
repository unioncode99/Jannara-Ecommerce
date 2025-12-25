using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.Address;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("{id:int}", Name = "GetAddressById")]
        public async Task<ActionResult<AddressDTO>> GetAddressById(int id)
        {
            var result = await _addressService.FindAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddressDTO>>> GetAllAddresses([FromQuery] int personId)
        {
            var result = await _addressService.GetAllAsync(personId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPost]
        public async Task<ActionResult<AddressDTO>> AddAddress(AddressCreateDTO request)
        {
            var result = await _addressService.AddNewAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateAddress(int id, [FromBody] AddressUpdateDTO request)
        {
            var result = await _addressService.UpdateAsync(id, request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAddress(int id)
        {
            var result = await _addressService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

    }
}
