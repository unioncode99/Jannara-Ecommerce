using Jannara_Ecommerce.DTOs.Dashboard;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IDashboardService
    {
        Task<Result<CustomerDashboardResponseDTO>> GetCustomerDashboardDataAsync(int customerId);
        Task<Result<AdminDashboardResponseDTO>> GetAdminDashboardDataAsync();
        Task<Result<SellerDashboardResponseDTO>> GetSellerDashboardDataAsync(int userId);
    }
}
