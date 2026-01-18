using Jannara_Ecommerce.DTOs.SellerProductImage;

namespace Jannara_Ecommerce.DTOs.SellerProduct
{
    public class SellerProductCreateDTO
    {
        public int? UserId { get; set; }
        public int ProductItemId { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        //public bool? IsActive { get; set; } = true;
        public IEnumerable<IFormFile>? SellerProductImages { get; set; }
        //public IEnumerable<SellerProductImageCreateDTO> SellerProductImages { get; set; }
    }
}
