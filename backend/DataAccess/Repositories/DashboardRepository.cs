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

        public async Task<Result<AdminDashboardResponseDTO>> GetAdminDashboardDataAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
-- Dashboard Query
SELECT 
    -- Total revenue
    (SELECT ISNULL(SUM(grand_total),0) FROM Orders WHERE order_status IN (2,3,4)) AS TotalRevenue,

    -- Active sellers
    (SELECT COUNT(*) 
     FROM Users u
     JOIN UserRoles ur ON u.id = ur.user_id
     JOIN Roles r ON ur.role_id = r.id
     WHERE r.name_en='Seller' AND ur.is_active=1) AS ActiveSellers,

    -- Total customers
    (SELECT COUNT(*) 
     FROM Users u
     JOIN UserRoles ur ON u.id = ur.user_id
     JOIN Roles r ON ur.role_id = r.id
     WHERE r.name_en='Customer' AND ur.is_active=1) AS TotalCustomers,

    -- Pending verifications
    (SELECT COUNT(*) 
     FROM Users u
     JOIN UserRoles ur ON u.id = ur.user_id
     JOIN Roles r ON ur.role_id = r.id
     WHERE (r.name_en='Seller' OR r.name_en='Customer') AND ur.is_active=1 AND u.is_confirmed=0) AS PendingVerifications,

    -- Last 5 registered users with roles
    LastUsers.JsonData AS LastRegisteredUsers,

    -- Monthly revenue last 12 months
    MonthlyRev.JsonData AS MonthlyRevenue

FROM 
    -- Last 5 users as JSON
    (SELECT TOP 5 
         u.id AS UserId,
         u.username,
         u.email,
         u.created_at AS RegisteredAt,
         p.first_name AS FirstName,
         p.last_name AS LastName,
         p.image_url AS ProfileImage,
         -- Roles per user
         (SELECT 
             UR.id AS Id,
             R.name_ar AS NameAr,
             R.name_en AS NameEn,
             UR.is_active AS IsActive,
             UR.created_at AS CreatedAt,
             UR.updated_at AS UpdatedAt
          FROM UserRoles UR
          JOIN Roles R ON UR.role_id = R.id
          WHERE UR.user_id = u.id
          FOR JSON PATH
         ) AS Roles
     FROM Users u
     LEFT JOIN People p ON p.id = u.person_id
     ORDER BY u.created_at DESC
     FOR JSON PATH
    ) AS LastUsers(JsonData)
CROSS APPLY
    -- Monthly revenue last 12 months
    (SELECT
         YEAR(created_at) AS [Year],
         MONTH(created_at) AS [Month],
         ISNULL(SUM(grand_total),0) AS Revenue
     FROM Orders
     WHERE order_status IN (2,3,4) AND created_at >= DATEADD(MONTH, -12, GETDATE())
     GROUP BY YEAR(created_at), MONTH(created_at)
     ORDER BY YEAR(created_at), MONTH(created_at)
     FOR JSON PATH
    ) AS MonthlyRev(JsonData)
FOR JSON PATH, WITHOUT_ARRAY_WRAPPER;



";

                using (var command = new SqlCommand(query, connection))
                {
                    try
                    {
                        await connection.OpenAsync();
                        using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);
                        {
                            if (!await reader.ReadAsync())
                            {
                                return new Result<AdminDashboardResponseDTO>(false, "Admin Data not found", null, 404);
                            }

                            // Read the entire JSON as a string first
                            string json = await reader.GetFieldValueAsync<string>(0);

                            var adminDashboardData = JsonSerializer.Deserialize<AdminDashboardResponseDTO>(
                                json,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                            );

                            return new Result<AdminDashboardResponseDTO>(true, "Admin Data fetched successfully", adminDashboardData, 200);

                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error fetching Admin Data : {ex}");
                        return new Result<AdminDashboardResponseDTO>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }

    }
}
