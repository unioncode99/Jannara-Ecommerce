namespace Jannara_Ecommerce.DTOs.Product
{
    public class ProductUpdateDTO
    {
        public int? BrandId { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? DefaultImageFile { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public decimal WeightKg { get; set; }
        public bool DeleteProductImage { get; set; }
    }
}
