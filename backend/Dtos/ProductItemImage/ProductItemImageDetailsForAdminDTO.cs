namespace Jannara_Ecommerce.DTOs.ProductItemImage
{
    public class ProductItemImageDetailsForAdminDTO
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
