using Jannara_Ecommerce.DTOs.ProductItemImage;

namespace Jannara_Ecommerce.DTOs.ProductItem
{
    public class ProductItemCreateDTO
    {
        public string Sku {  get; set; }
        public IEnumerable<ProductItemImageCreateDTO> ProductItemImages { get; set; }
  
    }
}
