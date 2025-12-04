using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.Business.Services;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.Enums;
using Jannara_Ecommerce.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Jannara_Ecommerce.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/sellers")]
    [ApiController]
    public class SellersController : ControllerBase
    {
        private readonly ISellerService _service;
        public SellersController(ISellerService sellerService)
        {
            _service = sellerService;
        }

        [HttpGet("{id}", Name = "GetSellerByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SellerDTO>> GetSellerByID(int id)
        {
            
            Result<SellerDTO> result = await _service.FindAsync(id);
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
        public async Task<ActionResult<SellerDTO>> AddSeller(SellerCreateRequestDTO request)
        {
            Result<SellerDTO> result = await _service.CreateAsync(request);
            if (result.IsSuccess)
            {
                return CreatedAtRoute("GetSellerByID", new { id = result.Data.Id }, result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SellerDTO>> UpdateSeller(int id, [FromBody] SellerUpdateDTO updatedSellerDTO)
        {
            
            Result<bool> result = await _service.UpdateAsync(id, updatedSellerDTO);
            if (result.IsSuccess)
            {
                return Ok(updatedSellerDTO);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteSeller(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Data");
            }
            Result<bool> result = await _service.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResponseDTO<SellerDTO>>> GetAll([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            if (pageNumber == 0 || pageSize == 0)
            {
                return BadRequest(new ResponseMessage("pageSize and pageNumber must be greater than zero."));
            }
            Result<PagedResponseDTO<SellerDTO>> result = await _service.GetAllAsync(pageNumber, pageSize);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

    }
}
