namespace Jannara_Ecommerce.DTOs.Product
{
    public class GeneralProductFilterDTO
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        // allowed values:
        //  newest | oldest
    }
}
