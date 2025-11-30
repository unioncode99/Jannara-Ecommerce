namespace Jannara_Ecommerce.DTOs
{
    public class UserDTO
    {
        public UserDTO(int id, int personId, string email, string username,
            string password, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            PersonId = personId;
            Email = email;
            Username = username;
            Password = password;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
