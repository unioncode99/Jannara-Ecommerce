namespace Jannara_Ecommerce.DTOs.ProductItemImage
{
    public class ProductItemImageDetailDTO
    {
        public int ProductItemImageId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
