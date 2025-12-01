using Jannara_Ecommerce.Enums;

namespace Jannara_Ecommerce.DTOs
{
    public class RegisteredSellerDTO
    {
        public RegisteredSellerDTO(string firstName, string lastName, string phone, IFormFile? profileImage, Gender gender, DateOnly dateOfBirth, string email, string username, string password, string businessName, string? websiteUrl)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            this.profileImage = profileImage;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            Email = email;
            Username = username;
            Password = password;
            BusinessName = businessName;
            WebsiteUrl = websiteUrl;
        }

        //Person Info
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public IFormFile? profileImage { get; set; }
        public Gender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        //User Info
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        //Seller Info
        public string BusinessName { get; set; }
        public string? WebsiteUrl { get; set; }
    }
}
