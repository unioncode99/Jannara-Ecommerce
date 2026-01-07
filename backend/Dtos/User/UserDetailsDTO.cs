using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.DTOs.Role;

namespace Jannara_Ecommerce.DTOs.User
{
    public class UserDetailsDTO
    {
        public int Id { get; set; }
        public int PersonId { get; set; }

        public string Email { get; set; }
        public string Username { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<RoleDetailsDTO> Roles { get; set; }
        public PersonDetailsDTO Person { get; set; }
    }
}
