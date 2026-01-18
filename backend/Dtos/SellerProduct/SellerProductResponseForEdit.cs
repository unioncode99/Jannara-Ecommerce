using Jannara_Ecommerce.DTOs.SellerProductImage;
using Microsoft.AspNetCore.Http.HttpResults;
using Stripe;
using static Azure.Core.HttpHeader;

namespace Jannara_Ecommerce.DTOs.SellerProduct
{
    public class SellerProductResponseForEdit
    {

        // Product 
        public int ProductId { get; set; }
        public string ProductNameEn { get; set; }
        public string ProductNameAr { get; set; }
        // Product Item
        public int ProductItemId { get; set; }
        public string Sku { get; set; }
        // Seller Product
        public int SellerProductId { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<SellerProductImageResponseForEdit> SellerProductImages { get; set; }
    }
}
