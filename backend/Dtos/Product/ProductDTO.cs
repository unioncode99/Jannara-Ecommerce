using System.Reflection.Metadata.Ecma335;

namespace Jannara_Ecommerce.DTOs.Product
{
    public class ProductDTO
    {
        public ProductDTO(int id, Guid publicId, int categoryId, int? brandId, string defaultImageUrl, string nameEn, string nameAr, string? descriptionEn, string? descriptionAr, decimal weightKg, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            PublicId = publicId;
            CategoryId = categoryId;
            BrandId = brandId;
            DefaultImageUrl = defaultImageUrl;
            NameEn = nameEn;
            NameAr = nameAr;
            DescriptionEn = descriptionEn;
            DescriptionAr = descriptionAr;
            WeightKg = weightKg;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public Guid PublicId { get; set; }
        public int CategoryId {get; set;}
        public int? BrandId { get; set; }
        public string DefaultImageUrl { get; set; }
        public string NameEn {  get; set; }
        public string NameAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public decimal WeightKg { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
