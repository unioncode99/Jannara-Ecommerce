namespace Jannara_Ecommerce.DTOs.SellerProductImage
{
    public class SellerProductImageCreateOneDTO
    {
        public int SellerProductId { get; set; }
        public IEnumerable<IFormFile> Images { get; set; }
    }
}
