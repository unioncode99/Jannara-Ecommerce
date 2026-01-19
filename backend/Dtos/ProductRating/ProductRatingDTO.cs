namespace Jannara_Ecommerce.DTOs.ProductRating
{
    public class ProductRatingDTO
    {

        public ProductRatingDTO(int id, int customerId, int productId, byte rating, string? reviewText, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            CustomerId = customerId;
            ProductId = productId;
            Rating = rating;
            ReviewText = reviewText;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public ProductRatingDTO()
        {

        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public byte Rating { get; set; }
        public string? ReviewText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
