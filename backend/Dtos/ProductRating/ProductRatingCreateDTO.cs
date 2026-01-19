using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs.ProductRating
{
    public class ProductRatingCreateDTO
    {
        public int? UserId { get; set; }
        public int ProductId { get; set; }
        [Range(1, 5)]
        public byte Rating { get; set; }
        public string? ReviewText { get; set; }
    }
}
