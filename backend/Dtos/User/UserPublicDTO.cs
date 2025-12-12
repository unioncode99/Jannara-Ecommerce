namespace Jannara_Ecommerce.DTOs.User
{
    public class UserPublicDTO
    {
        public UserPublicDTO(int id, int personId, string email, string username,
            DateTime createdAt, DateTime updatedAt, List<UserRoleInfoDTO> roles)
        {
            Id = id;
            PersonId = personId;
            Email = email;
            Username = username;
            Roles = roles;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<UserRoleInfoDTO> Roles { get; set; }   = new List<UserRoleInfoDTO>();
    }
}
