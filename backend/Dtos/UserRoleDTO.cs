namespace Jannara_Ecommerce.DTOs
{
    public class UserRoleDTO
    {
        public UserRoleDTO(int id, int roleId, int userId, bool isActive, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            RoleId = roleId;
            UserId = userId;
            this.isActive = isActive;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
