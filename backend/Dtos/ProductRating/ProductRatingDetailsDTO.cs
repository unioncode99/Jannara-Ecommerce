namespace Jannara_Ecommerce.DTOs.ProductRating
{
    public class ProductRatingDetailsDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ProfileImage { get; set; }
        public int ProductId { get; set; }
        public byte Rating { get; set; }
        public string? ReviewText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
