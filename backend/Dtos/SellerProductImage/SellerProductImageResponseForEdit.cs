namespace Jannara_Ecommerce.DTOs.SellerProductImage
{
    public class SellerProductImageResponseForEdit
    {
        public int Id { get; set; }

        public int SellerProductId { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
