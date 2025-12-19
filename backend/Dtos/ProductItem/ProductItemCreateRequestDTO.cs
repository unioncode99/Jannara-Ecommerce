using Jannara_Ecommerce.DTOs.Variation;
using Jannara_Ecommerce.DTOs.VariationOption;
using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs.ProductItem
{
    public class ProductItemCreateRequestDTO
    {
        // Product Info
        [Required]
        public int BrandId { get; set; }
        [Required]
        public string DefaultImageUrl { get; set; }
        [Required]
        public string NameEn { get; set; }
        [Required]
        public string NameAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        [Required]
        // Variations
        public IEnumerable<IDictionary<VariationCreateDTO, IEnumerable<VariationOptionCreateDTO>>> Variations { get; set; }
        [Required]
        // Product Items
        public IEnumerable<ProductItemCreateDTO> ProductItems { get; set; }

    }
}
