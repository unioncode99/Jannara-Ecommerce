namespace Jannara_Ecommerce.DTOs.Product
{
    public class ProductCreateDTO
    {
        public int BrandId { get; set; }
        public string DefaultImageUrl { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
    }
}
