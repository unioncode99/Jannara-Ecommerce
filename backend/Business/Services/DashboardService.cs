using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Dashboard;
using Jannara_Ecommerce.Utilities;
using Stripe;

namespace Jannara_Ecommerce.Business.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<Result<AdminDashboardResponseDTO>> GetAdminDashboardDataAsync()
        {
            return await _dashboardRepository.GetAdminDashboardDataAsync();
        }

        public async Task<Result<CustomerDashboardResponseDTO>> GetCustomerDashboardDataAsync(int customerId)
        {
            return await _dashboardRepository.GetCustomerDashboardDataAsync(customerId);
        }
    }
}
