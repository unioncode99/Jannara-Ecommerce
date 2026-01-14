using Jannara_Ecommerce.DTOs.ProductItem;
using Jannara_Ecommerce.DTOs.Variation;

namespace Jannara_Ecommerce.DTOs.Product
{
    public class ProductDetailsForAdminDTO
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
        public IEnumerable<VariationDetailsForAdminDTO> Variations { get; set; }
        public IEnumerable<ProductItemDetailsForAdminDTO> ProductItems { get; set; }
    }
}
