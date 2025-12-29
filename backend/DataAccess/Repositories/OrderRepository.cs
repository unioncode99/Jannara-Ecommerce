using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.DTOs.CartItem;
using Jannara_Ecommerce.DTOs.Order;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Org.BouncyCastle.Utilities.Zlib;
using Stripe;
using System.Data;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<IOrderRepository> _logger;
        public OrderRepository(IOptions<DatabaseSettings> options, ILogger<IOrderRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }

        public async Task<Result<OrderDTO>> CreateAsync(OrderCreateDTO orderCreateRequest)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
-- VARIABLES
DECLARE @orderId INT;
DECLARE @invoiceId INT;
DECLARE @subtotal DECIMAL(18,2);
DECLARE @taxCost DECIMAL(18,2);
DECLARE @shippingCost DECIMAL(18,2);
DECLARE @grandTotal DECIMAL(18,2);
DECLARE @totalWeight DECIMAL(18,2);
DECLARE @itemsCount INT;

-- BEGIN TRANSACTION
BEGIN TRANSACTION;

BEGIN TRY
	-- INPUT VALIDATIONS
    -- 1️ Validate Cart exists
    IF NOT EXISTS (SELECT 1 FROM Carts WHERE id = @cartId)
        THROW 50001, 'Cart does not exist.', 1;

    -- 2️ Validate Cart has items
    IF NOT EXISTS (SELECT 1 FROM CartItems WHERE cart_id = @cartId)
        THROW 50002, 'Cart is empty.', 1;

    -- 3️ Validate Customer exists
    IF NOT EXISTS (SELECT 1 FROM Customers WHERE id = @customerId)
        THROW 50003, 'Customer does not exist.', 1;

    -- 4️ Validate Payment Method exists
    IF NOT EXISTS (SELECT 1 FROM PaymentMethods WHERE id = @paymentMethodId)
        THROW 50004, 'Payment method is invalid.', 1;

    -- 5️ Validate Shipping Address belongs to customer
    IF NOT EXISTS (
    SELECT 1 
    FROM Addresses 
    WHERE id = @shippingAddressId 
      AND person_id = (SELECT person_id FROM Customers WHERE id = @customerId)
)
    THROW 50005, 'Shipping address is invalid for this customer.', 1;

    -- 6️ Validate Shipping Method exists
    IF NOT EXISTS (SELECT 1 FROM ShippingMethods WHERE id = @shippingMethodId)
        THROW 50006, 'Shipping method does not exist.', 1;

    -- 7️ Validate tax rate
    IF @taxRate < 0 OR @taxRate > 1
    THROW 50007, 'Tax rate must be between 0 and 1.', 1;

    -- 1️ CALCULATE SUBTOTAL, TOTAL WEIGHT, ITEMS COUNT
    SELECT 
   @subtotal = ISNULL(SUM(ci.quantity * ci.price_at_add_time), 0),
    @totalWeight = ISNULL(SUM(ci.[quantity] * p.[weight_kg]), 0),
    @itemsCount = ISNULL(SUM(ci.[quantity]), 0)
    FROM CartItems ci
    INNER JOIN SellerProducts sp ON sp.id = ci.seller_product_id
    INNER JOIN ProductItems pi ON pi.id = sp.product_item_id
    INNER JOIN Products p ON p.id = pi.product_id
    WHERE ci.cart_id = @cartId;

    -- 2️ FETCH SHIPPING METHOD INFO
    DECLARE @basePrice DECIMAL(18,2);
    DECLARE @pricePerKg DECIMAL(18,2);
    DECLARE @pricePerItem DECIMAL(18,2);
    DECLARE @freeOver DECIMAL(18,2);
    DECLARE @stateFee DECIMAL(18,2);

    SELECT 
        @basePrice = sm.base_price,
        @pricePerKg = sm.price_per_kg,
        @pricePerItem = sm.price_per_item,
        @freeOver = sm.free_over,
        @stateFee = ISNULL(st.[extra_fee_for_shipping],0)
    FROM ShippingMethods sm
    JOIN Addresses sa ON sa.[id] = @shippingAddressId
    JOIN States st ON st.[id] = sa.[state_id]
    WHERE sm.[id] = @shippingMethodId;

    -- 3️ CALCULATE SHIPPING COST
    IF @freeOver IS NOT NULL AND @subtotal >= @freeOver
        SET @shippingCost = 0;
    ELSE
        SET @shippingCost = ISNULL(@basePrice, 0)
                          + ISNULL(@pricePerKg, 0) * ISNULL(@totalWeight, 0)
                          + ISNULL(@pricePerItem, 0) * ISNULL(@itemsCount, 0)
                          + ISNULL(@stateFee, 0);

    -- 4️ CALCULATE TAX AND GRAND TOTAL
    SET @taxCost = ROUND(@subtotal * ISNULL(@taxRate, 0), 2); 
    SET @grandTotal = @subtotal + @taxCost + @shippingCost;

    -- 5️ INSERT ORDER
	-- 1 = Pending
    INSERT INTO Orders ([customer_id], [shipping_address_id], shipping_method_id, payment_intent_id, subtotal, [tax_cost], [shipping_cost], [grand_total], [order_status], [placed_at])
    VALUES (@customerId, @shippingAddressId, @shippingMethodId, @paymentIntentId, @subtotal, @taxCost, @shippingCost, @grandTotal, 1, GETDATE());

    SET @orderId = SCOPE_IDENTITY();

    -- 6️ INSERT ORDER ITEMS
    INSERT INTO OrderItems([order_id], [seller_product_id], [quantity], [unit_price], [created_at])
    SELECT 
        @orderId,
        ci.[seller_product_id],
        ci.[quantity],
        ci.price_at_add_time,
        GETDATE()
    FROM CartItems ci
    WHERE ci.[cart_id] = @cartId;

    -- 7️ INSERT INVOICE
	-- 1 = Paid, 2 = Unpaid
    INSERT INTO [invoices] ([order_id], [customer_id], invoice_number, [invoice_status], [created_at])
    VALUES (@orderId, @customerId, @orderId + 100, CASE WHEN @payNow = 1 THEN 1 ELSE 2 END, GETDATE());

    SET @invoiceId = SCOPE_IDENTITY();

    -- 8️ INSERT PAYMENT IF PAYNOW = 1
    IF @payNow = 1
    BEGIN
        INSERT INTO [payments] ([invoice_id], [paid_by_customer_id], payment_method_id, [amount], is_paid, [paid_at], [transaction_reference], [created_at])
        VALUES (
            @invoiceId,
            @customerId,
            @paymentMethodId,
            @grandTotal,
            1,
            GETDATE(),
            @transactionReference,
            GETDATE()
        );
    END

    -- deactivate cart
    Update carts set is_active = 0 where id = @cartId;

    -- create new cart for customer
    INSERT INTO Carts(customer_id, is_active)
    VALUES (@customerId, 1);
    

    -- 9️ COMMIT TRANSACTION
    COMMIT TRANSACTION;

    -- 10️ RETURN RESULTS
   -- SELECT 
       -- @orderId AS [orderId], 
        --@invoiceId AS [invoiceId], 
       -- CASE WHEN @payNow = 1 THEN 'paid' ELSE 'unpaid' END AS [invoiceStatus],
       -- @subtotal AS [subtotal],
       -- @taxCost AS [tax],
--@shippingCost AS [shippingCost],
       -- @grandTotal AS [grandTotal];
select * from orders where id = @orderId;

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
END CATCH
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@cartId", orderCreateRequest.CartId);
                    command.Parameters.AddWithValue("@customerId", orderCreateRequest.CustomerId);
                    command.Parameters.AddWithValue("@paymentMethodId", orderCreateRequest.PaymentMethodId);
                    command.Parameters.AddWithValue("@shippingAddressId", orderCreateRequest.ShippingAddressId);
                    command.Parameters.AddWithValue("@shippingMethodId", orderCreateRequest.ShippingMethodId);
                    command.Parameters.AddWithValue("@payNow", orderCreateRequest.PayNow);
                    command.Parameters.AddWithValue("@taxRate", orderCreateRequest.TaxRate);
                    command.Parameters.AddWithValue("@transactionReference", orderCreateRequest.TransactionReference ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@paymentIntentId", orderCreateRequest.PaymentIntentId);

                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var insertedOrder = new OrderDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetString(reader.GetOrdinal("public_order_id")),
                                    reader.GetInt32(reader.GetOrdinal("customer_id")),
                                    reader.GetInt32(reader.GetOrdinal("shipping_address_id")),
                                    reader.GetInt32(reader.GetOrdinal("shipping_method_id")),
                                    reader.GetString(reader.GetOrdinal("payment_intent_id")),
                                    reader.GetByte(reader.GetOrdinal("order_status")),
                                    reader.GetDecimal(reader.GetOrdinal("subtotal")),
                                    reader.GetDecimal(reader.GetOrdinal("tax_cost")),
                                    reader.GetDecimal(reader.GetOrdinal("shipping_cost")),
                                    reader.GetDecimal(reader.GetOrdinal("grand_total")),
                                    reader.GetDateTime(reader.GetOrdinal("placed_at")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                );
                                return new Result<OrderDTO>(true, "order_added_successfully", insertedOrder);
                            }
                            return new Result<OrderDTO>(false, "failed_to_add_order", null, 500);
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        string message = sqlEx.Number switch
                        {
                            50001 => "Cart does not exist",
                            50002 => "Cart is empty",
                            50003 => "Customer does not exist",
                            50004 => "Payment method is invalid",
                            50005 => "Shipping address is invalid for this customer",
                            50006 => "Shipping method does not exist",
                            50007 => "Tax rate must be between 0 and 1",
                            _ => "internal_server_error"
                        };
                        _logger.LogError(sqlEx, "SQL exception in CreateAsync");
                        return new Result<OrderDTO>(false, message, null, 400);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unexpected error in CreateAsync");
                        return new Result<OrderDTO>(false, "internal_server_error", null, 500);
                    }
                }
            }

        }

        public async Task<Result<OrderDTO>> ConfirmPaymentAsync(int? orderId, string? paymentIntentId, int paymentMethodId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
DECLARE @invoiceId INT;
DECLARE @customerId INT;
DECLARE @orderId INT;
DECLARE @amount DECIMAL(18,2);

BEGIN TRANSACTION;

BEGIN TRY
    -- 1️ Get order by orderId and paymentIntentId
    SELECT 
        @orderId = id,
        @customerId = customer_id,
        @amount = grand_total
    FROM Orders
    WHERE id = @order_id OR payment_intent_id = @paymentIntentId;

    -- Validate order exists
    IF @customerId IS NULL
        THROW 50001, 'Order not found for the given ID and payment intent.', 1;

    -- 2️ Get invoice for this order
    SELECT @invoiceId = id
    FROM Invoices
    WHERE order_id = @orderId;

    IF @invoiceId IS NULL
        THROW 50002, 'Invoice not found for this order.', 1;

    -- 3️ Prevent duplicate payment
    IF EXISTS (SELECT 1 FROM Payments WHERE invoice_id = @invoiceId AND is_paid = 1)
        THROW 50003, 'Order is already paid.', 1;

    -- 4️ Insert payment
    INSERT INTO Payments
    (
        invoice_id,
        paid_by_customer_id,
        payment_method_id,
        amount,
        is_paid,
        paid_at,
        transaction_reference,
        created_at,
        updated_at
    )
    VALUES
    (
        @invoiceId,
        @customerId,
        @paymentMethodId,
        @amount,
        1,
        GETDATE(),
        @paymentIntentId,
        GETDATE(),
        GETDATE()
    );

    -- 5️ Update order status to Paid (2)
    UPDATE Orders
    SET order_status = 2,
        updated_at = GETDATE()
    WHERE id = @orderId;

    COMMIT TRANSACTION;

    -- 6️ Return order info
    SELECT * FROM Orders WHERE id = @orderId;

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
END CATCH
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@order_id", orderId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@paymentIntentId", paymentIntentId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@paymentMethodId", paymentMethodId);
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var order = new OrderDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetString(reader.GetOrdinal("public_order_id")),
                                    reader.GetInt32(reader.GetOrdinal("customer_id")),
                                    reader.GetInt32(reader.GetOrdinal("shipping_address_id")),
                                    reader.GetInt32(reader.GetOrdinal("shipping_method_id")),
                                    reader.GetString(reader.GetOrdinal("payment_intent_id")),
                                    reader.GetByte(reader.GetOrdinal("order_status")),
                                    reader.GetDecimal(reader.GetOrdinal("subtotal")),
                                    reader.GetDecimal(reader.GetOrdinal("tax_cost")),
                                    reader.GetDecimal(reader.GetOrdinal("shipping_cost")),
                                    reader.GetDecimal(reader.GetOrdinal("grand_total")),
                                    reader.GetDateTime(reader.GetOrdinal("placed_at")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                );
                                return new Result<OrderDTO>(true, "payment_completed_successfully", order);
                            }
                            return new Result<OrderDTO>(false, "order_not_found", null, 404);
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        string message = sqlEx.Number switch
                        {
                            50001 => "Order not found for the given ID and payment intent",
                            50002 => "Invoice not found for this order",
                            50003 => "Order is already paid",
                            _ => "internal_server_error"
                        };

                        _logger.LogError(sqlEx, "SQL exception in CompletePaymentAsync");
                        return new Result<OrderDTO>(false, message, null, 400);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unexpected error in UpdateStatusAsync");
                        return new Result<OrderDTO>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }


        public async Task<Result<OrderDetailsDTO>> GetByPublicIdAsync(string publicId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
DECLARE @json NVARCHAR(MAX);
DECLARE @orderId INT = 46

SELECT @json = (
    SELECT
        o.id AS Id,
        o.public_order_id AS PublicOrderId,
        o.customer_id AS CustomerId,
        o.shipping_address_id AS ShippingAddressId,
        o.shipping_method_id AS ShippingMethodId,
        o.payment_intent_id AS PaymentIntentId,
        o.order_status AS OrderStatus,

        CASE o.order_status
            WHEN 1 THEN 'Pending'
            WHEN 2 THEN 'Processing'
            WHEN 3 THEN 'Shipped'
            WHEN 4 THEN 'Delivered'
            WHEN 5 THEN 'Cancelled'
            ELSE 'Unknown'
        END AS StatusNameEn,

        CASE o.order_status
            WHEN 1 THEN N'قيد الانتظار'
            WHEN 2 THEN N'قيد المعالجة'
            WHEN 3 THEN N'تم الشحن'
            WHEN 4 THEN N'تم التوصيل'
            WHEN 5 THEN N'ملغى'
            ELSE N'غير معروف'
        END AS StatusNameAr,

        o.subtotal AS SubTotal,
        o.tax_cost AS TaxCost,
        o.shipping_cost AS ShippingCost,
        o.grand_total AS GrandTotal,
        o.placed_at AS PlacedAt,
        o.created_at AS CreatedAt,
        o.updated_at AS UpdatedAt,

        -- Nested JSON for OrderItems
        (
            SELECT
                oi.id AS Id,
                oi.seller_product_id AS SellerProductId,
                oi.quantity AS Quantity,
                oi.unit_price AS UnitPrice,
                oi.total_price AS TotalPrice,

                p.name_en AS NameEn,
                p.name_ar AS NameAr,
                p.default_image_url AS DefaultImageUrl,
                pi.sku AS Sku,

                oi.created_at AS CreatedAt,
                oi.updated_at AS UpdatedAt
            FROM OrderItems oi
            Left join SellerProducts sp
            on sp.id = oi.seller_product_id
            Left join ProductItems pi
            on sp.product_item_id = pi.id
            Left join Products p
            on pi.product_id = p.id
            WHERE oi.order_id = o.id
            FOR JSON PATH
        ) AS OrderItems
    FROM Orders o
    WHERE o.public_order_id = @publicId
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
);

SELECT @json AS FullJson;
";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@publicId", publicId);
                    try
                    {
                        await connection.OpenAsync();
                        using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);
                        {
                            if (!await reader.ReadAsync())
                                return new Result<OrderDetailsDTO>(false, "order not found", null, 404);

                            // Read the entire JSON as a string first
                            string json = await reader.GetFieldValueAsync<string>(0);

                            var product = JsonSerializer.Deserialize<OrderDetailsDTO>(
                                json,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                            );

                            return new Result<OrderDetailsDTO>(true, "order fetched successfully", product, 200);

                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error fetching order by id {publicId}: {ex}");
                        return new Result<OrderDetailsDTO>(false, "Error fetching order", null, 500);
                    }
                }
            }
        }

        public async Task<Result<IEnumerable<OrderDetailsDTO>>> GetCustomerOrdersAsync(int customerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
DECLARE @json NVARCHAR(MAX);
SELECT @json = (
    SELECT
        o.id AS Id,
        o.public_order_id AS PublicOrderId,
        o.customer_id AS CustomerId,
        o.shipping_address_id AS ShippingAddressId,
        o.shipping_method_id AS ShippingMethodId,
        o.payment_intent_id AS PaymentIntentId,
        o.order_status AS OrderStatus,

        CASE o.order_status
            WHEN 1 THEN 'Pending'
            WHEN 2 THEN 'Processing'
            WHEN 3 THEN 'Shipped'
            WHEN 4 THEN 'Delivered'
            WHEN 5 THEN 'Cancelled'
            ELSE 'Unknown'
        END AS StatusNameEn,

        CASE o.order_status
            WHEN 1 THEN N'قيد الانتظار'
            WHEN 2 THEN N'قيد المعالجة'
            WHEN 3 THEN N'تم الشحن'
            WHEN 4 THEN N'تم التوصيل'
            WHEN 5 THEN N'ملغى'
            ELSE N'غير معروف'
        END AS StatusNameAr,

        o.subtotal AS SubTotal,
        o.tax_cost AS TaxCost,
        o.shipping_cost AS ShippingCost,
        o.grand_total AS GrandTotal,
        o.placed_at AS PlacedAt,
        o.created_at AS CreatedAt,
        o.updated_at AS UpdatedAt,

        -- Nested JSON for OrderItems (all items, no seller filter)
        (
            SELECT
                oi.id AS Id,
                oi.seller_product_id AS SellerProductId,
                oi.quantity AS Quantity,
                oi.unit_price AS UnitPrice,
                oi.total_price AS TotalPrice,

                p.name_en AS NameEn,
                p.name_ar AS NameAr,
                p.default_image_url AS DefaultImageUrl,
                pi.sku AS Sku,

                oi.created_at AS CreatedAt,
                oi.updated_at AS UpdatedAt
            FROM OrderItems oi
            LEFT JOIN SellerProducts sp
                ON sp.id = oi.seller_product_id
            LEFT JOIN ProductItems pi
                ON sp.product_item_id = pi.id
            LEFT JOIN Products p
                ON pi.product_id = p.id
            WHERE oi.order_id = o.id
            FOR JSON PATH
        ) AS OrderItems
    FROM Orders o
    WHERE o.customer_id = @customerId -- filter by customer
    FOR JSON PATH
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
                                return new Result<IEnumerable<OrderDetailsDTO>>(false, "customer orders not found", null, 404);

                            // Read the entire JSON as a string first
                            string json = await reader.GetFieldValueAsync<string>(0);

                            var product = JsonSerializer.Deserialize<IEnumerable<OrderDetailsDTO>>(
                                json,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                            );

                            return new Result<IEnumerable<OrderDetailsDTO>>(true, "customer orders fetched successfully", product, 200);

                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error fetching customer orders : {ex}");
                        return new Result<IEnumerable<OrderDetailsDTO>>(false, "Error fetching customer orders", null, 500);
                    }
                }
            }
        }

    }
}
