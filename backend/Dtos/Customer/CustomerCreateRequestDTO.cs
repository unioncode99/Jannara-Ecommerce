using Jannara_Ecommerce.Enums;

namespace Jannara_Ecommerce.DTOs.Customer
{
    public class CustomerCreateRequestDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public Gender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }

        // user info
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
