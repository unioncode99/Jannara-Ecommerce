using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs.Authentication
{
    public class ConfirmAccountRequestDTO
    {
        [Required]
        public string Token { get; set; }
    }
}
