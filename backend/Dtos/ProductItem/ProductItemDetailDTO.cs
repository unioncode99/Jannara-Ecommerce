using Jannara_Ecommerce.DTOs.ProductItemImage;
using Jannara_Ecommerce.DTOs.ProductItemVariationOption;
using Jannara_Ecommerce.DTOs.SellerProduct;
using Jannara_Ecommerce.DTOs.Variation;

namespace Jannara_Ecommerce.DTOs.ProductItem
{
    public class ProductItemDetailDTO
    {
        public int ProductItemId { get; set; }
        public string Sku { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<ProductItemVariationOptionDetailDTO> ProductItemVariationOptions { get; set; }
        public List<ProductItemImageDetailDTO> ProductItemImages { get; set; }
        public List<SellerProductDetailDTO> SellerProducts { get; set; }
    }
}
