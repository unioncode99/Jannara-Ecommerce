using Jannara_Ecommerce.DTOs.Role;

namespace Jannara_Ecommerce.DTOs.Dashboard
{
    public class AdminDashboardResponseDTO
    {
        public decimal TotalRevenue { get; set; }
        public int ActiveSellers { get; set; }
        public int TotalCustomers { get; set; }
        public int PendingVerifications { get; set; }

        // Last 5 registered users
        public IEnumerable<RecentUserDTO> LastRegisteredUsers { get; set; }

        // Monthly revenue (last 12 months)
        public IEnumerable<MonthlyRevenueDTO> MonthlyRevenue { get; set; }
    }
}
