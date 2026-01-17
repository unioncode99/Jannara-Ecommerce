namespace Jannara_Ecommerce.DTOs.SellerProduct
{
    public class SellerProductResponseDTO
    {
        // Product
        public int ProductId { get; set; }
        public string ProductNameEn { get; set; }
        public string ProductNameAr { get; set; }
        public string ProductImage { get; set; }

        // Seller Product
        public int SellerProductId { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        // Category
        public string CategoryNameEn { get; set; }
        public string CategoryNameAr { get; set; }

        // Brand
        public string BrandNameEn { get; set; }
        public string BrandNameAr { get; set; }
    }
}
