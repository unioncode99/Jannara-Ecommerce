using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Cart;
using Jannara_Ecommerce.DTOs.CartItem;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Transactions;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ICartItemRepository> _logger;
        public CartItemRepository(IOptions<DatabaseSettings> options, ILogger<ICartItemRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }

        public async Task<Result<CartItemDTO>> AddOrUpdateAsync(CartItemRequestDTO cartItemRequest)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                }
                catch (SqlException sqlEx)
                {
                    _logger.LogError(sqlEx, "Failed to open SQL connection.");
                    return new Result<CartItemDTO>(false, "internal_server_error", null, 500);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error opening SQL connection.");
                    return new Result<CartItemDTO>(false, "internal_server_error", null, 500);
                }

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
DECLARE @CartId INT;
DECLARE @InsertedCartItems TABLE
(
    id INT,
    cart_id INT,
    seller_product_id INT,
    quantity TINYINT,
    price_at_add_time DECIMAL(18,2),
    subtotal DECIMAL(18,2),
    created_at DATETIME2,
    updated_at DATETIME2
);

-- Get active cart
SELECT TOP 1 @CartId = id
FROM Carts
WHERE customer_id = @CustomerId AND is_active = 1;

-- Create new cart if not found
IF @CartId IS NULL
BEGIN
    INSERT INTO Carts(customer_id, is_active)
    VALUES (@CustomerId, 1);
    SET @CartId = SCOPE_IDENTITY();
END

-- Deactivate all other carts
UPDATE Carts
SET is_active = 0
WHERE customer_id = @CustomerId
  AND id <> @CartId
  AND is_active = 1;

-- Check stock availability
DECLARE @AvailableStock INT;
SELECT @AvailableStock = stock_quantity
FROM SellerProducts
WHERE id = @SellerProductId;

IF @AvailableStock IS NULL
BEGIN
    -- SellerProduct not found
    THROW 50000, 'Seller product does not exist.', 1;
END

IF @Quantity > @AvailableStock
BEGIN
    -- Not enough stock
    THROW 50001, 'Not enough stock available.', 1;
END

-- Add or update cart item
DECLARE @CartItemId INT;
SELECT @CartItemId = id
FROM CartItems
WHERE cart_id = @CartId AND seller_product_id = @SellerProductId;

IF @CartItemId IS NOT NULL
BEGIN
    UPDATE CartItems
    SET quantity = @Quantity,
        updated_at = SYSDATETIME()
    OUTPUT inserted.*
    INTO @InsertedCartItems
    WHERE id = @CartItemId;
END
ELSE
BEGIN
    INSERT INTO CartItems(cart_id, seller_product_id, quantity, price_at_add_time)
    OUTPUT inserted.*
    INTO @InsertedCartItems
    VALUES(@CartId, @SellerProductId, @Quantity, (SELECT price FROM SellerProducts WHERE id = @SellerProductId));
END

-- Return the inserted/updated row
SELECT *
FROM @InsertedCartItems;

";
                        using (var command = new SqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@CustomerId", cartItemRequest.CustomerId);
                            command.Parameters.AddWithValue("@SellerProductId", cartItemRequest.SellerProductId);
                            command.Parameters.AddWithValue("@Quantity", cartItemRequest.Quantity);

                            CartItemDTO insertedCartItem = null;
                            using (var reader = await command.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    insertedCartItem = new CartItemDTO
                                    (
                                        reader.GetInt32(reader.GetOrdinal("id")),
                                        reader.GetInt32(reader.GetOrdinal("cart_id")),
                                        reader.GetInt32(reader.GetOrdinal("seller_product_id")),
                                        reader.GetByte(reader.GetOrdinal("quantity")),
                                        reader.GetDecimal(reader.GetOrdinal("price_at_add_time")),
                                        reader.GetDecimal(reader.GetOrdinal("subtotal")),
                                        reader.GetDateTime(reader.GetOrdinal("created_at")),
                                        reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                    );


                                }

                            }
                            if (insertedCartItem != null)
                            {
                                await transaction.CommitAsync();
                                return new Result<CartItemDTO>(true, "cart_item_added_successfully", insertedCartItem, 200);
                            }
                            else
                            {
                                await transaction.RollbackAsync();
                                return new Result<CartItemDTO>(false, "failed_to_add_cart_item", null, 500);
                            }
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        // Catch SQL THROW from stock check or product not found
                        await transaction.RollbackAsync();
                        string message = sqlEx.Number switch
                        {
                            50000 => "seller_product_does_not_exist",
                            50001 => "not_enough_stock_available",
                            _ => "internal_server_error"
                        };
                        _logger.LogError(sqlEx, "SQL exception in AddOrUpdateAsync");
                        return new Result<CartItemDTO>(false, message, null, 400);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(ex, "Unexpected error in AddOrUpdateAsync");
                        return new Result<CartItemDTO>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = @"
Delete from CartItems Where id = @id
";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        int rowAffected = await command.ExecuteNonQueryAsync();
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "cart_item_deleted_successfully", true, 200);
                        }
                        return new Result<bool>(false, "failed_to_delete_cart_item", false, 500);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "failed to delete cart Item");
                    return new Result<bool>(false, "internal_server_error", false, 500);
                }
            }
            
        }
    }
}
