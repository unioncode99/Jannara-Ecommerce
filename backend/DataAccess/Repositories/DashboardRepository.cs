using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Dashboard;
using Jannara_Ecommerce.DTOs.Order;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Stripe;
using System.Data;
using System.Text.Json;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<IDashboardRepository> _logger;
        public DashboardRepository(IOptions<DatabaseSettings> options, ILogger<IDashboardRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }

        public async Task<Result<CustomerDashboardResponseDTO>> GetCustomerDashboardDataAsync(int customerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
DECLARE @json NVARCHAR(MAX);

SELECT @json = (
    SELECT
        (SELECT COUNT(*) FROM Orders WHERE customer_id = @customerId) AS TotalOrders,
        (SELECT ISNULL(SUM(grand_total), 0) FROM Orders WHERE customer_id = @customerId) AS TotalSpent,
        (SELECT COUNT(*) FROM Orders WHERE customer_id = @customerId AND order_status = 1) AS TotalPendingOrders,
        
        -- Latest 5 Orders
        (SELECT TOP 5
            o.id As Id,
            o.public_order_id AS PublicOrderId,
            o.grand_total as GrandTotal,
			ItemsCount = (select count(*) from OrderItems where order_id = o.id),
            CASE order_status
                WHEN 1 THEN 'Pending'
                WHEN 2 THEN 'Processing'
                WHEN 3 THEN 'Shipped'
                WHEN 4 THEN 'Delivered'
                WHEN 5 THEN 'Cancelled'
                ELSE 'Unknown'
            END AS StatusNameEn,
            CASE order_status
                WHEN 1 THEN N'قيد الانتظار'
                WHEN 2 THEN N'قيد المعالجة'
                WHEN 3 THEN N'تم الشحن'
                WHEN 4 THEN N'تم التوصيل'
                WHEN 5 THEN N'ملغى'
                ELSE N'غير معروف'
            END AS StatusNameAr,
            o.placed_at AS PlacedAt
         FROM Orders o
         WHERE customer_id = @customerId
         ORDER BY o.placed_at DESC
         FOR JSON PATH) AS LatestOrders,

        -- Wishlist Items
        (SELECT TOP 5
            p.id as Id,
            p.public_id as PublicId,
            p.name_en as ProductNameEn,
			p.name_ar as ProductNameAr,
            p.default_image_url as ProductImageUrl,
			MinPrice = ( select Min(sp.price) from SellerProducts sp Left JOin ProductItems pi
			on sp.product_item_id = pi.id 
			where pi.product_id = p.id)
         FROM CustomerWishlist sw
		Left JOin Products p
		on sw.product_id = p.id
         WHERE sw.customer_id = @customerId
         FOR JSON PATH) AS Wishlist
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
);

SELECT @json AS FullJson;
";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@customerId", customerId);
                    try
                    {
                        await connection.OpenAsync();
                        using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);
                        {
                            if (!await reader.ReadAsync())
                            {
                                return new Result<CustomerDashboardResponseDTO>(false, "Customer Data not found", null, 404);
                            }

                            // Read the entire JSON as a string first
                            string json = await reader.GetFieldValueAsync<string>(0);

                            var customerDashboardData = JsonSerializer.Deserialize<CustomerDashboardResponseDTO>(
                                json,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                            );

                            return new Result<CustomerDashboardResponseDTO>(true, "Customer Data fetched successfully", customerDashboardData, 200);

                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error fetching Customer Data : {ex}");
                        return new Result<CustomerDashboardResponseDTO>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }
    
    }
}
