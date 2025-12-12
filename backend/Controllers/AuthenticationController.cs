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
    }
}
