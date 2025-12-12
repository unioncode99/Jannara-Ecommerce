using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs.Token;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Utilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Jannara_Ecommerce.Business.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;
        public TokenService(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }
        public AccessTokenDTO GenerateAccessToken(UserDTO user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.NameEn));
            }


            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                IssuedAt = DateTime.UtcNow,               
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.LifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)),
                SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return new AccessTokenDTO(tokenHandler.WriteToken(securityToken), DateTime.UtcNow.AddMinutes(_jwtOptions.LifeTime), DateTime.UtcNow);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];         
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}
