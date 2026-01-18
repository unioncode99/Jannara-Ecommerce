using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Order;
using Jannara_Ecommerce.DTOs.SellerOrder;
using Jannara_Ecommerce.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Stripe;
using System.Text.Json;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class SellerOrderRepository : ISellerOrderRepository
    {

        private readonly string _connectionString;
        private readonly ILogger<ISellerOrderRepository> _logger;
        public SellerOrderRepository(IOptions<DatabaseSettings> options, ILogger<ISellerOrderRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }

        public async Task<Result<PagedResponseDTO<SellerOrderResponseDTO>>> GetSellerOrdersAsync(SellerOrderFilterDTO filter)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"

DECLARE @json NVARCHAR(MAX);
DECLARE @SellerId int;

set @SellerId = (select id from sellers where user_id = @userId)

select Count(so.id) as total 
from SellerOrders so
where so.seller_id = @SellerId

AND (
    @SearchTerm IS NULL
    OR so.public_order_id LIKE '%' + @SearchTerm + '%'
);

SELECT @json = 
(
-- SellerOrders
select 
so.id as Id,
so.public_order_id as PublicId,
so.grand_total as GrandTotal,
so.created_at as CreatedAt,
so.order_status AS OrderStatus,
CASE so.order_status
    WHEN 1 THEN 'Pending'
    WHEN 2 THEN 'Processing'
    WHEN 3 THEN 'Shipped'
    WHEN 4 THEN 'Delivered'
    WHEN 5 THEN 'Cancelled'
    ELSE 'Unknown'
END AS StatusNameEn,
CASE so.order_status
    WHEN 1 THEN N'قيد الانتظار'
    WHEN 2 THEN N'قيد المعالجة'
    WHEN 3 THEN N'تم الشحن'
    WHEN 4 THEN N'تم التوصيل'
    WHEN 5 THEN N'ملغى'
    ELSE N'غير معروف'
END AS StatusNameAr,
-- SellerOrderItems
(
select 
soi.id as SellerOrderItemId,
soi.quantity as Quantity,
soi.unit_price as UnitPrice,
soi.total_price as TotalPrice,
p.name_en as ProductNameEn,
p.name_ar as ProductNameAr,
p.default_image_url as DefaultImageUrl
from 
SellerOrderItems soi
Join SellerProducts sp on sp.id = soi.seller_product_id
JOIN ProductItems pi on pi.id = sp.product_item_id
JOIN Products p  on p.id = pi.product_id
where soi.seller_order_id = so.id
FOR JSON PATH
) AS SellerOrderItems,

-- Seller info
JSON_QUERY((
SELECT
    c.id AS Id,
    u.id AS UserId,
    u.email AS Email,
    p.first_name AS FirstName,
    p.last_name AS LastName,
    p.phone AS Phone,
    p.image_url as ImageUrl

FROM Customers c
LEFT JOIN Users u
    ON u.id = c.user_id
LEFT JOIN People p
    ON p.id = u.person_id
WHERE c.id = o.customer_id
FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
)) AS Customer

from SellerOrders so
left join Orders o on o.id = so.customer_order_id
where so.seller_id = @SellerId

AND (
    @SearchTerm IS NULL
    OR so.public_order_id LIKE '%' + @SearchTerm + '%'
)
ORDER BY
    CASE WHEN @SortBy = 'price_asc' THEN so.grand_total END ASC,
    CASE WHEN @SortBy = 'price_desc' THEN so.grand_total END DESC,
    CASE WHEN @SortBy = 'newest' THEN so.id END DESC,
    CASE WHEN @SortBy = 'oldest' THEN so.id END ASC,
    o.id DESC
OFFSET (@PageNumber - 1) * @PageSize ROWS
FETCH NEXT @PageSize ROWS ONLY

FOR JSON PATH
);

SELECT @json AS FullJson;

";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", filter.UserId);
                    command.Parameters.AddWithValue("@SearchTerm", (object?)filter.SearchTerm ?? DBNull.Value);
                    command.Parameters.AddWithValue("@SortBy", (object?)filter.SortBy ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PageNumber", filter.PageNumber);
                    command.Parameters.AddWithValue("@PageSize", filter.PageSize);
                    try
                    {
                        await connection.OpenAsync();
                        using var reader = await command.ExecuteReaderAsync();
                        {
                            if (!await reader.ReadAsync())
                            {
                                return new Result<PagedResponseDTO<SellerOrderResponseDTO>>(
                                    false, "customer_orders_not_found", null, 404);
                            }

                            // Total
                            int total = reader.GetInt32(0);
                            await reader.NextResultAsync();
                            await reader.ReadAsync();


                            // Read the entire JSON as a string first
                            string json = reader.IsDBNull(0)
    ? "[]"
    : reader.GetString(0);

                            var orders = JsonSerializer.Deserialize<IEnumerable<SellerOrderResponseDTO>>(
                                json,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                            );
                            var response = new PagedResponseDTO<SellerOrderResponseDTO>(total, filter.PageNumber, filter.PageSize, orders);
                            return new Result<PagedResponseDTO<SellerOrderResponseDTO>>(true, "products_retrieved_successfully", response, 200);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error fetching customer orders : {ex}");
                        return new Result<PagedResponseDTO<SellerOrderResponseDTO>>(false, "Error fetching customer orders", null, 500);

                    }
                }
            }
        }


        public async Task<Result<bool>> UpdateOrderStatusAsync(ChangeSellerOrderStatusRequest request)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE SellerOrders
                SET order_status = @NewStatus
                WHERE (id = @orderId OR public_order_id = @publicId)
                  AND order_status in (1, 2, 3, 4)
AND @NewStatus > order_status;
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@orderId", request.OrderId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@publicId", request.PublicId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@NewStatus", request.OrderStatus);
                    try
                    {
                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected == 0)
                        {
                            return new Result<bool>(false, "can not cancel order", false, 400);
                        }

                        return new Result<bool>(true, "order_cancelled_successfully", true, 200);
                    }

                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unexpected error in CancelOrder");
                        return new Result<bool>(false, "can not internal_server_error order", false, 500);
                    }
                }
            }
        }

    }
}
