using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs.Authentication
{
    public class ForgetPasswordRequestDTO
    {
        [Required]
        public string Email { get; set; }
    }
}
