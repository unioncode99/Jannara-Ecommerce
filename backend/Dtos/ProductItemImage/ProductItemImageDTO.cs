namespace Jannara_Ecommerce.DTOs.ProductItemImage
{
    public class ProductItemImageDTO
    {
        public ProductItemImageDTO(int id, int productItemId, string imageUrl, bool isPrimary, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            ProductItemId = productItemId;
            ImageUrl = imageUrl;
            IsPrimary = isPrimary;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public int ProductItemId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
