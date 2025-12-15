using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs.Authentication
{
    public class ResendVerificationCodeRequestDTO
    {
        [Required]
        public string Email { get; set; }
    }
}
