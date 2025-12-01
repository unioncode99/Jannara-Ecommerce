namespace Jannara_Ecommerce.DTOs
{
    public class CreateNewUserDTO
    {
        public CreateNewUserDTO(UserDTO user, PersonDTO person)
        {
            User = user;
            Person = person;
        }

        public UserDTO User {  get; set; }
        public PersonDTO Person { get; set; }
    }
}
