using Jannara_Ecommerce.DTOs.ProductItemImage;
using Jannara_Ecommerce.DTOs.VariationOption;

namespace Jannara_Ecommerce.DTOs.ProductItem
{
    public class ProductItemCreateDBDTO
    {
        public string Sku { get; set; }
        public IEnumerable<VariationOptionCreateDTO> VariationOptions { get; set; }
        public IEnumerable<ProductItemImageCreateDBDTO> ProductItemImages { get; set; }
    }
}
