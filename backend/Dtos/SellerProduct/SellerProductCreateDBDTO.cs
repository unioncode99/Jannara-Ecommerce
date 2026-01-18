namespace Jannara_Ecommerce.DTOs.SellerProduct
{
    public class SellerProductCreateDBDTO
    {
        public int SellerId { get; set; }
        public int ProductItemId { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
