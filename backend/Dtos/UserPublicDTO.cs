using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "PersonId is required.")]
        public int PersonId { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email format is invalid.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
