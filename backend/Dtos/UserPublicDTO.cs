namespace Jannara_Ecommerce.DTOs
{
    public class UserPublicDTO
    {
        public UserPublicDTO(int id, int personId, string email, string username, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            PersonId = personId;
            Email = email;
            Username = username;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
