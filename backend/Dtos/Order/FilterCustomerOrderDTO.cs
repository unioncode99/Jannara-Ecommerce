namespace Jannara_Ecommerce.DTOs.Order
{
    public class FilterCustomerOrderDTO
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int? UserId { get; set; }
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        // allowed values:
        // price_asc | price_desc | newest | oldest
    }
}
