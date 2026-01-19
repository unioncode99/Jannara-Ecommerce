namespace Jannara_Ecommerce.DTOs.ProductRating
{
    public class ProductRatingUpdateDTO
    {
        public int Id { get; set; }
        public byte Rating { get; set; }
        public string? ReviewText { get; set; }
    }
}
