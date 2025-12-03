using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonService _service;
        public PeopleController(IPersonService service)
        {
            _service = service;
        }
        [HttpGet("{id:int}", Name = "GetPersonByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonDTO>> GetPersonById(int id)
        {
            Result<PersonDTO> result = await _service.FindAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdatePerson(int id,[FromForm] PersonUpdateDTO updatedPerson)
        {
            Result<bool> result = await _service.UpdateAsync(id, updatedPerson);

            if (!result.IsSuccess)
                return StatusCode(result.ErrorCode, result.Message);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeletePerson(int id)
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
