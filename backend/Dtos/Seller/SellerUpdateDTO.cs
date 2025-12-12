using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs.Seller
{
    public class SellerUpdateDTO
    {
        [Required(ErrorMessage = "Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than zero")]
        public int Id { get; set; }
        [Required(ErrorMessage = "BusinessName is required.")]
        public string BusinessName { get; set; }
        public string? WebsiteUrl { get; set; }
    }
}
