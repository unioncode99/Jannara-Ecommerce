namespace Jannara_Ecommerce.DTOs.SellerProductImage
{
    public class SellerProductImageDTO
    {
        public SellerProductImageDTO(int id, int sellerProductId, string imageUrl, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            SellerProductId = sellerProductId;
            ImageUrl = imageUrl;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public int SellerProductId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
