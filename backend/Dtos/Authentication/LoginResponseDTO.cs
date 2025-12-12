using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.DTOs.Token;
using Jannara_Ecommerce.DTOs.User;

namespace Jannara_Ecommerce.DTOs.Authentication
{
    public class LoginResponseDTO
    {
        public LoginResponseDTO(PersonDTO person, UserPublicDTO user, AccessTokenDTO accessToken)
        {
            Person = person;
            User = user;
            AccessToken = accessToken;
        }

        public PersonDTO Person { get; set; }
        public UserPublicDTO User { get; set; }
        public AccessTokenDTO AccessToken  { get; set; }
    }
}
