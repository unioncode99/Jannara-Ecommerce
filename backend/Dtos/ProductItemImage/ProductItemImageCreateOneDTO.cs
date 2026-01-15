namespace Jannara_Ecommerce.DTOs.ProductItemImage
{
    public class ProductItemImageCreateOneDTO
    {
        public int ProductItemId { get; set; }
        public IEnumerable<ProductItemImageCreateDTO> ProductItemImages { get; set; }
    }
}
