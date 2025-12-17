using Jannara_Ecommerce.DTOs.Authentication;

namespace Jannara_Ecommerce.Utilities.WrapperClasses
{
    public class LoginResult
    {
        public LoginResult(LoginResponseDTO loginResponse, string refreshToken)
        {
            LoginResponse = loginResponse;
            RefreshToken = refreshToken;
        }

        public LoginResponseDTO LoginResponse {  get; set; }
        public string RefreshToken { get; set; }
    }
}
