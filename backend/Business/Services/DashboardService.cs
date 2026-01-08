using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Dashboard;
using Jannara_Ecommerce.Utilities;
using Microsoft.Extensions.Options;
using Stripe;

namespace Jannara_Ecommerce.Business.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly string _baseUrl;
        public DashboardService(IDashboardRepository dashboardRepository, IOptions<AppSettings> appSettings)
        {
            _dashboardRepository = dashboardRepository;
            _baseUrl = appSettings.Value.BaseUrl;
        }

        public async Task<Result<AdminDashboardResponseDTO>> GetAdminDashboardDataAsync()
        {
            var adminDashboardResult = await _dashboardRepository.GetAdminDashboardDataAsync();
            if (adminDashboardResult.IsSuccess)
            {
                foreach (var item in adminDashboardResult?.Data?.LastRegisteredUsers)
                {
                    item.ProfileImage = ImageUrlHelper.ToAbsoluteUrl(item.ProfileImage, _baseUrl);
                }
            }
            return adminDashboardResult;
        }

        public async Task<Result<CustomerDashboardResponseDTO>> GetCustomerDashboardDataAsync(int customerId)
        {
            return await _dashboardRepository.GetCustomerDashboardDataAsync(customerId);
        }
    }
}
