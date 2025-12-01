using Jannara_Ecommerce.Enums;

namespace Jannara_Ecommerce.DTOs
{
    public class RegisteredCustomerDTO
    {
        public RegisteredCustomerDTO(PersonDTO person, UserDTO user, CustomerDTO customer)
        {
            Person = person;
            User = user;
            Customer = customer;
        }

        public PersonDTO Person { get; set; }
        public UserDTO User { get; set; }
        public CustomerDTO Customer { get; set; }
    }
}
