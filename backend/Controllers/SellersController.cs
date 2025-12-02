using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.Business.Services;
using Jannara_Ecommerce.DTOs;
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
        private readonly ISellerService _sellerService;
        public SellersController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        [HttpGet("{id}", Name = "GetSellerByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SellerDTO>> GetSellerByID(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Data");
            }
            Result<SellerDTO> result = await _sellerService.FindAsync(id);
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
        public async Task<ActionResult<SellerDTO>> AddSeller(RegisteredSellerDTO registeredSellerDTO)
        {
            if
            (
                string.IsNullOrWhiteSpace(registeredSellerDTO.Person.FirstName) ||
                string.IsNullOrWhiteSpace(registeredSellerDTO.Person.LastName) ||
                string.IsNullOrWhiteSpace(registeredSellerDTO.Person.Phone) ||
                registeredSellerDTO.Person.DateOfBirth == default ||
                (registeredSellerDTO.Person.Gender != Gender.Male && registeredSellerDTO.Person.Gender != Gender.Female && registeredSellerDTO.Person.Gender != Gender.Other) ||
                string.IsNullOrWhiteSpace(registeredSellerDTO.User.Email) ||
                string.IsNullOrWhiteSpace(registeredSellerDTO.User.Username) ||
                string.IsNullOrWhiteSpace(registeredSellerDTO.User.Password) ||
                !Regex.IsMatch(registeredSellerDTO.User.Email ?? "", @"^[^@\s]+@[^@\s]+\.[^@\s]+$") ||
                string.IsNullOrWhiteSpace(registeredSellerDTO.Seller.BusinessName)
            )
            {
                return BadRequest("Invalid Data");
            }
            Result<SellerDTO> result = await _sellerService.RegisterAsync(registeredSellerDTO);
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
        public async Task<ActionResult<SellerDTO>> UpdateSeller(int id, [FromBody] SellerDTO updatedSellerDTO)
        {
            if (string.IsNullOrWhiteSpace(updatedSellerDTO.BusinessName) || id <= 0)
            {
                return BadRequest("Invalid Data");
            }
            Result<bool> result = await _sellerService.UpdateAsync(id, updatedSellerDTO);
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
            Result<bool> result = await _sellerService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return StatusCode(result.ErrorCode, result.Message);
        }

    }
}
