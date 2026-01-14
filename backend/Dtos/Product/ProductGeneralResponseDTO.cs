namespace Jannara_Ecommerce.DTOs.Product
{
    public class ProductGeneralResponseDTO
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; }
        public int CategoryId { get; set; }
        public int? BrandId { get; set; }
        public string DefaultImageUrl { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public decimal WeightKg { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CategoryNameEn { get; set; }
        public string CategoryNameAr { get; set; }
        public string BrandNameEn { get; set; }
        public string BrandNameAr { get; set; }
    }
}
