using Jannara_Ecommerce.DTOs.Brand;
using Jannara_Ecommerce.DTOs.ProductItem;
using Jannara_Ecommerce.DTOs.Variation;

namespace Jannara_Ecommerce.DTOs.Product
{
    public class ProductDetailDTO
    {
        public int ProductId { get; set; }
        public Guid PublicId { get; set; }
        public string DefaultImageUrl { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal MinPrice { get; set; }
        public bool? IsFavorite { get; set; }
        public decimal? AverageRating { get; set; }
        public int? RatingCount { get; set; }
        public BrandDetailDTO? Brand { get; set; }
        public List<VariationDetailDTO> Variations { get; set; }
        public List<ProductItemDetailDTO> ProductItems { get; set; }
    }
}
