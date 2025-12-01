using Jannara_Ecommerce.Enums;
using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs
{
    public class RegisteredSellerDTO
    {
        public RegisteredSellerDTO(PersonDTO person, UserDTO user, SellerDTO seller)
        {
            Person = person;
            User = user;
            Seller = seller;
        }

        public PersonDTO Person { get; set; }
        public UserDTO User { get; set; }
        public SellerDTO Seller { get; set; }
    }
}
