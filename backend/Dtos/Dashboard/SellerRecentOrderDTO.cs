namespace Jannara_Ecommerce.DTOs.Dashboard
{
    public class SellerRecentOrderDTO
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string StatusNameEn { get; set; } = string.Empty;
        public string StatusNameAr { get; set; } = string.Empty;
    }
}
