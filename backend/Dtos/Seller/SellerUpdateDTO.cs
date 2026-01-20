using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs.Seller
{
    public class SellerUpdateDTO
    {
        public int? CurrentUserId { get; set; }
        [Required(ErrorMessage = "BusinessName is required.")]
        public string BusinessName { get; set; }
        public string? WebsiteUrl { get; set; }
    }
}
