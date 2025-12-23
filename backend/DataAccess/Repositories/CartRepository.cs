using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Cart;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;
using System.Text.Json;
using System.Transactions;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ICartRepository> _logger;
        public CartRepository(IOptions<DatabaseSettings> options, ILogger<ICartRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }

        public async Task<Result<bool>> ClearCartAsync(int cartId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = @"
Delete from CartItems Where cart_id = @cartId
";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cartId", cartId);
                        int rowAffected = await command.ExecuteNonQueryAsync();
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "cart_cleared_successfully", true, 200);
                        }
                        return new Result<bool>(false, "failed_to_clear_cart", false, 500);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "failed to clear cart");
                    return new Result<bool>(false, "internal_server_error", false, 500);
                }
            }
        }

        public async Task<Result<CartResponseDTO>> GetActiveCartAsync(int customerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
DECLARE @json NVARCHAR(MAX);

SELECT @json = (
    SELECT
        c.id AS Id,
        c.customer_id AS CustomerId,
        c.is_active AS IsActive,
        c.created_at AS CreatedAt,
        c.updated_at AS UpdatedAt,
        (SELECT COUNT(*) FROM CartItems ci WHERE ci.cart_id = c.id) AS ItemsCount,
        (SELECT SUM(ci.quantity * ci.price_at_add_time) FROM CartItems ci WHERE ci.cart_id = c.id) AS TotalPrice,
        (
            SELECT
                ci.id AS Id,
                ci.seller_product_id AS SellerProductId,
                ci.quantity AS Quantity,
                ci.price_at_add_time AS PriceAtAddTime,
                ci.quantity * ci.price_at_add_time AS SubTotal,
                ci.created_at AS CreatedAt,
                ci.updated_at AS UpdatedAt
            FROM CartItems ci
            WHERE ci.cart_id = c.id
            FOR JSON PATH
        ) AS CartItems
    FROM Carts c
    WHERE c.customer_id = @customerId AND c.is_active = 1
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
                                return new Result<CartResponseDTO>(false, "cart_not_found", null, 404);
                            }
                            // Read the entire JSON as a string first
                            string json = await reader.GetFieldValueAsync<string>(0);

                            var cart = JsonSerializer.Deserialize<CartResponseDTO>(
                                json,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                            );

                            return new Result<CartResponseDTO>(true, "cart_fetched_successfully", cart, 200);

                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error fetching cart: {ex}");
                        return new Result<CartResponseDTO>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }
    }
}
