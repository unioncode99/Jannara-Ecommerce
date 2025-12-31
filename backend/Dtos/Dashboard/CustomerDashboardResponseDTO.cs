using Jannara_Ecommerce.DTOs.Order;
using Jannara_Ecommerce.DTOs.Wishlist;

namespace Jannara_Ecommerce.DTOs.Dashboard
{
    public class CustomerDashboardResponseDTO
    {
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public int TotalPendingOrders { get; set; }
        public List<LatestCustomerOrderDTO> LatestOrders { get; set; }
        public List<WishlistItemDTO> Wishlist { get; set; }
    }
}
