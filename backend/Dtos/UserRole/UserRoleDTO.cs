using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs.UserRole
{
    public class UserRoleDTO
    {
        public UserRoleDTO(int id, int roleId, int userId, bool isActive, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            RoleId = roleId;
            UserId = userId;
            IsActive = isActive;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "RoleId is required.")]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "isActive is required.")]
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
