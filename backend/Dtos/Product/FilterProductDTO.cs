namespace Jannara_Ecommerce.DTOs.Product
{
    public class FilterProductDTO
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int? CustomerId { get; set; }
        public int? CategoryId { get; set; }
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        // allowed values:
        // price_asc | price_desc | newest | oldest
    }
}
