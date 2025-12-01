using Jannara_Ecommerce.DTOs;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IPasswordService
    {
        public string HashPassword(UserDTO user);
        public bool VerifyPassword(UserDTO user, string enteredPassword);
    }
}
