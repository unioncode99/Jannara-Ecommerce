using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.DTOs.User;

namespace Jannara_Ecommerce.DTOs.Authentication
{
    public class LoginResponseDTO<T> where T : class
    {
        public LoginResponseDTO(PersonDTO person, UserPublicDTO user, Dictionary<string, T> roleData)
        {
            Person = person;
            User = user;
            RoleData = roleData;
        }

        public PersonDTO Person { get; set; }
        public UserPublicDTO User { get; set; }
        public Dictionary<string, T> RoleData { get; set; }
    }
}
