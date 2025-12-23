using Jannara_Ecommerce.DTOs.VariationOption;

namespace Jannara_Ecommerce.DTOs.CartItem
{
    public class CartItemResponseDTO
    {
        public int Id { get; set; }
        public int SellerProductId { get; set; }
        public byte Quantity { get; set; }
        public decimal PriceAtAddTime { get; set; }
        public decimal SubTotal { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // Product 
        public string ProductNameEn { get; set; }
        public string ProductNameAr { get; set; }
        public string DefaultProductImage { get; set; }
        // Category
        public string CategoryNameEn { get; set; }
        public string CategoryNameAr { get; set; }
        // Brand 
        public string BrandNameEn { get; set; }
        public string BrandNameAr { get; set; }
        // SKU
        public string Sku { get; set; }
        // Stock
        public int AvailableStock { get; set; }
        public IEnumerable<VariationOptionCreateDTO> SelectedOptions { get; set; }
    }
}
