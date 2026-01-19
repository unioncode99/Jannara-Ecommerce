namespace Jannara_Ecommerce.DTOs.ProductRating
{
    public class ProductRatingFilterDTO
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int ProductId { get; set; }
    }
}
