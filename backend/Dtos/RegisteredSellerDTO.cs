using Jannara_Ecommerce.Enums;

namespace Jannara_Ecommerce.DTOs
{
    public class RegisteredSellerDTO
    {
        public RegisteredSellerDTO(PersonDTO person, UserDTO user, string businessName, string? websiteUrl)
        {
            Person = person;
            User = user;
            BusinessName = businessName;
            WebsiteUrl = websiteUrl;
        }

        public PersonDTO Person { get; set; }
        public UserDTO User { get; set; }
        //Seller Info
        public string BusinessName { get; set; }
        public string? WebsiteUrl { get; set; }
    }
}
