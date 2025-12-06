using Jannara_Ecommerce.Business.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Jannara_Ecommerce.Business.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<object> _hasher = new PasswordHasher<object>();
  
        public string HashPassword<T>(T user, string password) where T : class 
        {
            if (password == "") return "";
            return _hasher.HashPassword(user, password);
        }


        public bool VerifyPassword <T>(T user, string password, string enteredPassword) where T : class
        {
            PasswordVerificationResult result = _hasher.VerifyHashedPassword(user, password, enteredPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
