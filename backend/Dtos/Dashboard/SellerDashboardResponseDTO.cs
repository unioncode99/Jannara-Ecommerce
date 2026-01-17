namespace Jannara_Ecommerce.DTOs.Dashboard
{
    public class SellerDashboardResponseDTO
    {
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public int ActiveProducts { get; set; }
        public List<SellerRecentOrderDTO> RecentOrders { get; set; } = new();
        public List<MonthlyRevenueDTO> MonthlyRevenue { get; set; } = new();
    }
}
