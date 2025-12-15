using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs.Authentication
{
    public class ResendAccountConfirmationRequest
    {
        [Required]
        public string Email { get; set; }
    }
}
