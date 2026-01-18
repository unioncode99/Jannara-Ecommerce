using Jannara_Ecommerce.DTOs.SellerProductImage;

namespace Jannara_Ecommerce.DTOs.SellerProduct
{
    public class SellerProductResponseForEdit
    {
        public int Id { get; set; }

        public int ProductItemId { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public IEnumerable<SellerProductImageResponseForEdit> SellerProductImages { get; set; }
    }
}
