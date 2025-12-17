using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs.User
{
    public class UserUpdateDTO
    {
        [Required(ErrorMessage = "Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than zero")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
