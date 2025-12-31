namespace Jannara_Ecommerce.DTOs.Order
{
    public class LatestCustomerOrderDTO
    {
        public int Id { get; set; }
        public string PublicOrderId { get; set; }
        public decimal GrandTotal { get; set; }
        public int ItemsCount { get; set; }
        public string StatusNameEn { get; set; }
        public string StatusNameAr { get; set; }
        public DateTime PlacedAt { get; set; }
    }
}
