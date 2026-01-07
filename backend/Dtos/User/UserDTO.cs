using Jannara_Ecommerce.DTOs.UserRole;
using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs.User
{
    public class UserDTO
    {
        public UserDTO(int id, int personId, string email, string username, bool isConfirmed, string password, DateTime createdAt, DateTime updatedAt, List<UserRoleInfoDTO> roles)
        {
            Id = id;
            PersonId = personId;
            Email = email;
            Username = username;
            IsConfirmed = isConfirmed;
            Password = password;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Roles = roles;
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "PersonId is required.")]
        public int PersonId { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email format is invalid.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public bool IsConfirmed { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<UserRoleInfoDTO> Roles { get; set; }
    }
}
