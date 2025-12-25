using Jannara_Ecommerce.DTOs.SellerProductImage;

namespace Jannara_Ecommerce.DTOs.SellerProduct
{
    public class SellerProductDetailDTO
    {
        public int SellerProductId { get; set; }
        public int SellerId { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<SellerProductImageDetailDTO> SellerProductImages { get; set; } = new();
  
    }
}
