using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs.UserRole
{
    public class UserRoleUpdateDTO
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public int? UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
