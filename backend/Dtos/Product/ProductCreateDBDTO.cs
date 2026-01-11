using Jannara_Ecommerce.DTOs.ProductItem;
using Jannara_Ecommerce.DTOs.Variation;

namespace Jannara_Ecommerce.DTOs.Product
{
    public class ProductCreateDBDTO
    {
        public int? BrandId { get; set; }
        public string DefaultImageUrl { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public decimal WeightKg { get; set; }
        public IEnumerable<VariationCreateDTO> Variations { get; set; }
        public IEnumerable<ProductItemCreateDBDTO> ProductItems { get; set; }
    }
}
