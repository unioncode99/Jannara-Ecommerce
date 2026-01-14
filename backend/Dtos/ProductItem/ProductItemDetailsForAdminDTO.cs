using Jannara_Ecommerce.DTOs.ProductItemImage;
using Jannara_Ecommerce.DTOs.ProductItemVariationOption;
using Jannara_Ecommerce.DTOs.VariationOption;

namespace Jannara_Ecommerce.DTOs.ProductItem
{
    public class ProductItemDetailsForAdminDTO
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<ProductItemVariationDetailsForAdminDTO> VariationOptions { get; set; }
        public IEnumerable<ProductItemImageDetailsForAdminDTO> ProductItemImages { get; set; }
    }
}
