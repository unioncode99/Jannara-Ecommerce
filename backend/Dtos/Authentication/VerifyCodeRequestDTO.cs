using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs.Authentication
{
    public class VerifyCodeRequestDTO
    {
        [Required]
        public string Code { get; set; }
    }
}
