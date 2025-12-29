namespace Jannara_Ecommerce.DTOs.OrderItem
{
    public class OrderItemDetailsDTO
    {
        public int Id { get; set; }
        public int SellerProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string DefaultImageUrl { get; set; }
        public string Sku { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
