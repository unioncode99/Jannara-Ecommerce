namespace Jannara_Ecommerce.DTOs.SellerOrderItem
{
    public class SellerOrderItemResponseDTO
    {
        public int SellerOrderItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string ProductNameEn { get; set; }
        public string ProductNameAr { get; set; }
        public string DefaultImageUrl { get; set; }
    }
}
