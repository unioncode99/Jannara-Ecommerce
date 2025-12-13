using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.Authentication;
using Jannara_Ecommerce.Utilities;
using Jannara_Ecommerce.Utilities.WrapperClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jannara_Ecommerce.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _service;

        public AuthenticationController(IAuthenticationService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login(LoginDTO request)
        {
            Result<LoginResult> result = await _service.LogInAsync(request);

            if (!result.IsSuccess)
                return StatusCode(result.ErrorCode, result.Message);
            Response.Cookies.Append("refreshToken", result.Data.RefreshToken, new CookieOptions
            {
                HttpOnly = true,                  
                Secure = true,                    
                SameSite = SameSiteMode.Strict,   
                Expires = DateTime.UtcNow.AddDays(7) 
            });
            return Ok(result.Data.LoginResponse);
        }

        [HttpPost("forget-password")]
        public async Task<ActionResult> ForgetPassword([FromBody] string email)
        {
            Result<bool> result = await _service.ForgetPasswordAsync(email);
            if (result.IsSuccess)
            {
                return Ok();
            }
            return StatusCode(result.ErrorCode, result.Message);
        }


        [HttpPost("reset-password")]
        public async Task<ActionResult<LoginResponseDTO>> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            Result<bool> result = await _service.ResetPasswordAsync(resetPasswordDTO);

            if (result.IsSuccess)
                return Ok(result.Data);
            return StatusCode(result.ErrorCode, result.Message);
        }

        [HttpPost("verify-reset-Code")]
        public async Task<ActionResult<string>> VerifyResetCode([FromBody] string resetCode)
        {
            Result<string> result = await _service.VerifyResetCodeAsync(resetCode);

            if (result.IsSuccess)
                return Ok(result.Data);
            return StatusCode(result.ErrorCode, result.Message);
        }
    }
}
