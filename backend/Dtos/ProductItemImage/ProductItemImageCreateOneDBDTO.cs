namespace Jannara_Ecommerce.DTOs.ProductItemImage
{
    public class ProductItemImageCreateOneDBDTO
    {
        public int ProductItemId { get; set; }
        public IEnumerable<ProductItemImageCreateDBDTO> ProductItemImages { get; set; }
    }
}
