using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Jannara_Ecommerce.Business.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<object> _hasher = new PasswordHasher<object>();
        public string HashPassword(UserDTO user)
        {
            if (user.Password == "") return "";
            return _hasher.HashPassword(user, user.Password);
        }

        public bool VerifyPassword(UserDTO user, string enteredPassword)
        {
            PasswordVerificationResult result = _hasher.VerifyHashedPassword(user, user.Password, enteredPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
