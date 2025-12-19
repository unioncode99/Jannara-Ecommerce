namespace Jannara_Ecommerce.DTOs.ProductItemImage
{
    public class ProductItemImageCreateDTO
    {
        public IFormFile ImageFile { get; set; }
        public bool IsPrimary { get; set; }
    }
}
