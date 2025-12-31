using Jannara_Ecommerce.DTOs.Dashboard;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IDashboardRepository
    {
        Task<Result<CustomerDashboardResponseDTO>> GetCustomerDashboardDataAsync(int customerId);
    }
}
