namespace Jannara_Ecommerce.DTOs.Brand
{
    public class BrandUpdateDTO
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string LogoUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
    }
}
