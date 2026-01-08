using Jannara_Ecommerce.DTOs.Role;

namespace Jannara_Ecommerce.DTOs.Dashboard
{
    public class RecentUserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfileImage { get; set; }
        public IEnumerable<RoleDetailsDTO> Roles { get; set; }
    }
}
