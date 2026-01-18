using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.DTOs.SellerProduct;
using Jannara_Ecommerce.DTOs.SellerProductImage;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Utilities.Zlib;
using System.Data;
using System.Text.Json;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class SellerProductRepository : ISellerProductRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ISellerProductRepository> _logger;
        public SellerProductRepository(IOptions<DatabaseSettings> options, ILogger<ISellerProductRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }

        public async Task<Result<PagedResponseDTO<SellerProductResponseDTO>>> GetAllAsync(SellerProductFilterDTO filter)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"
DECLARE @SellerId INT = 1;

SET @SellerId = (SELECT TOP 1 id FROM Sellers WHERE user_id = @UserId);

SELECT COUNT(DISTINCT sp.id) AS total
from SellerProducts sp
Join ProductItems pi on sp.product_item_id = pi.id
Join Products p on p.id = pi.product_id
Left Join ProductCategories pc on pc.id = p.category_id
Left Join Brands b on b.id = p.brand_id
WHERE
sp.seller_id = @SellerId AND
(@CategoryId IS NULL OR p.category_id = @CategoryId)
AND (@BrandId IS NULL OR p.brand_id = @BrandId)
AND (
        @SearchTerm IS NULL
        OR p.name_en LIKE '%' + @SearchTerm + '%'
        OR p.name_ar LIKE '%' + @SearchTerm + '%'
        OR p.description_en LIKE '%' + @SearchTerm + '%'
        OR p.description_ar LIKE '%' + @SearchTerm + '%'
		OR pc.name_en LIKE '%' + @SearchTerm + '%'
		OR pc.name_ar LIKE '%' + @SearchTerm + '%'
		OR pc.description_en LIKE '%' + @SearchTerm + '%'
		OR pc.description_ar LIKE '%' + @SearchTerm + '%'
		OR b.name_en LIKE '%' + @SearchTerm + '%'
		OR b.name_ar LIKE '%' + @SearchTerm + '%'
		OR b.description_en LIKE '%' + @SearchTerm + '%'
		OR b.description_ar LIKE '%' + @SearchTerm + '%'
    )


select 
-- Product
p.id As ProductId,
p.name_en as ProductNameEn,
p.name_ar as ProductNameAr, 
p.default_image_url as ProductImage,
-- Product Item
pi.sku as Sku,
-- Seller Product
sp.id as SellerProductId,
sp.stock_quantity as StockQuantity,
sp.price as Price,
sp.is_active as IsActive,
-- Category
pc.name_en As CategoryNameEn,
pc.name_ar As CategoryNameAr,
-- Category
b.name_en As BrandNameEn,
b.name_ar As BrandNameAr
from SellerProducts sp
Join ProductItems pi on sp.product_item_id = pi.id
Join Products p on p.id = pi.product_id
Left Join ProductCategories pc on pc.id = p.category_id
Left Join Brands b on b.id = p.brand_id
WHERE
sp.seller_id = @SellerId AND
    (@CategoryId IS NULL OR p.category_id = @CategoryId)
AND (@BrandId IS NULL OR p.brand_id = @BrandId)
AND (
        @SearchTerm IS NULL
        OR p.name_en LIKE '%' + @SearchTerm + '%'
        OR p.name_ar LIKE '%' + @SearchTerm + '%'
        OR p.description_en LIKE '%' + @SearchTerm + '%'
        OR p.description_ar LIKE '%' + @SearchTerm + '%'
		OR pc.name_en LIKE '%' + @SearchTerm + '%'
		OR pc.name_ar LIKE '%' + @SearchTerm + '%'
		OR pc.description_en LIKE '%' + @SearchTerm + '%'
		OR pc.description_ar LIKE '%' + @SearchTerm + '%'
		OR b.name_en LIKE '%' + @SearchTerm + '%'
		OR b.name_ar LIKE '%' + @SearchTerm + '%'
		OR b.description_en LIKE '%' + @SearchTerm + '%'
		OR b.description_ar LIKE '%' + @SearchTerm + '%'
    )
ORDER BY
    CASE WHEN @SortBy = 'newest' THEN sp.id END DESC,
    CASE WHEN @SortBy = 'oldest' THEN sp.id END ASC,
    sp.id DESC
OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;

";

            using var command = new SqlCommand(query, connection);

            int offset = (filter.PageNumber - 1) * filter.PageSize;
            command.Parameters.AddWithValue("@offset", offset);
            command.Parameters.AddWithValue("@pageSize", filter.PageSize);
            command.Parameters.AddWithValue("@UserId", filter.UserId);
            command.Parameters.AddWithValue("@SortBy", filter.SortBy ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CategoryId", filter.CategoryId ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@BrandId", filter.BrandId ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@SearchTerm", filter.SearchTerm ?? (object)DBNull.Value);

            try
            {
                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();

                int total = 0;
                if (await reader.ReadAsync())
                {
                    total = reader.GetInt32(reader.GetOrdinal("total"));
                }

                if (!await reader.NextResultAsync() || !reader.HasRows)
                {
                    return new Result<PagedResponseDTO<SellerProductResponseDTO>>(false, "products_not_found", null, 404);
                }

                var products = new List<SellerProductResponseDTO>();
                while (await reader.ReadAsync())
                {
                    products.Add(new SellerProductResponseDTO
                    {

                        ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                        ProductNameEn = reader.GetString(reader.GetOrdinal("ProductNameEn")),
                        ProductNameAr = reader.GetString(reader.GetOrdinal("ProductNameAr")),
                        ProductImage = reader.GetString(reader.GetOrdinal("ProductImage")),

                        Sku = reader.GetString(reader.GetOrdinal("Sku")),

                        SellerProductId = reader.GetInt32(reader.GetOrdinal("SellerProductId")),
                        StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),

                        CategoryNameEn = reader.GetString(reader.GetOrdinal("CategoryNameEn")),
                        CategoryNameAr = reader.GetString(reader.GetOrdinal("CategoryNameAr")),

                        BrandNameEn = reader.GetString(reader.GetOrdinal("BrandNameEn")),
                        BrandNameAr = reader.GetString(reader.GetOrdinal("BrandNameAr"))

                    });
                }

                var response = new PagedResponseDTO<SellerProductResponseDTO>(total, filter.PageNumber, filter.PageSize, products);
                return new Result<PagedResponseDTO<SellerProductResponseDTO>>(true, "products_retrieved_successfully", response, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve products for page {PageNumber} with page size {PageSize}", filter.PageNumber, filter.PageSize);
                return new Result<PagedResponseDTO<SellerProductResponseDTO>>(false, "internal_server_error", null, 500);
            }
        }

        public async Task<Result<SellerProductDTO>> AddNewAsync(SellerProductCreateDBDTO product, SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"
DECLARE @SellerId INT;

SET @SellerId = (SELECT TOP 1 id FROM Sellers WHERE user_id = @UserId);

INSERT INTO [dbo].[SellerProducts]
(
    [seller_id],
    [product_item_id],
    [price],
    [stock_quantity],
    [is_active]
)
OUTPUT inserted.*
VALUES
(
    @SellerId,
    @ProductItemId,
    @Price,
    @StockQuantity,
    @IsActive
);
";

            using var command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@UserId", product.UserId);
            command.Parameters.AddWithValue("@ProductItemId", product.ProductItemId);
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
            command.Parameters.AddWithValue("@IsActive", true);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var insertedProduct = new SellerProductDTO
                (
                    reader.GetInt32(reader.GetOrdinal("id")),
                    reader.GetInt32(reader.GetOrdinal("seller_id")),
                    reader.GetInt32(reader.GetOrdinal("product_item_id")),
                    reader.GetDecimal(reader.GetOrdinal("price")),
                    reader.GetInt32(reader.GetOrdinal("stock_quantity")),
                    reader.GetBoolean(reader.GetOrdinal("is_active")),
                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                );
                return new Result<SellerProductDTO>(true, "product_added_successfully", insertedProduct);
            }

            return new Result<SellerProductDTO>(false, "failed_to_add_product", null, 500);
        }

        public async Task<Result<SellerProductDTO>> UpdateAsync(int id, SellerProductUpdateDTO productUpdateDBDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"

UPDATE [dbo].[SellerProducts]
SET
    [price] = @Price,
    [stock_quantity] = @StockQuantity,
    [is_active] = @IsActive
WHERE
    [id] = @Id;

select * from SellerProducts where id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", productUpdateDBDTO.Id);
                    command.Parameters.AddWithValue("@Price", productUpdateDBDTO.Price);
                    command.Parameters.AddWithValue("@StockQuantity", productUpdateDBDTO.StockQuantity);
                    command.Parameters.AddWithValue("@IsActive", productUpdateDBDTO.IsActive);

                    try
                    {
                        await connection.OpenAsync();
                        using var reader = await command.ExecuteReaderAsync();
                        if (await reader.ReadAsync())
                        {
                            var insertedProduct = new SellerProductDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetInt32(reader.GetOrdinal("seller_id")),
                                reader.GetInt32(reader.GetOrdinal("product_item_id")),
                                reader.GetDecimal(reader.GetOrdinal("price")),
                                reader.GetInt32(reader.GetOrdinal("stock_quantity")),
                                reader.GetBoolean(reader.GetOrdinal("is_active")),
                                reader.GetDateTime(reader.GetOrdinal("created_at")),
                                reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            );
                            return new Result<SellerProductDTO>(true, "product_added_successfully", insertedProduct);
                        }

                        return new Result<SellerProductDTO>(false, "failed_to_add_product", null, 500);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to update person with PersonId {PersonId}", id);
                        return new Result<SellerProductDTO>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM SellerProducts WHERE id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    try
                    {
                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return new Result<bool>(true, "role_deleted_successfully", true);
                        }
                        return new Result<bool>(false, "role_not_found", false, 404);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to delete role with RoleId {RoleId}", id);
                        return new Result<bool>(false, "internal_server_error", false, 500);
                    }
                }
            }

        }

        public async Task<Result<SellerProductResponseForEdit>> GetSellerProductForEditAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
DECLARE @json NVARCHAR(MAX);
SELECT @json = (
    SELECT
	    -- Product
		p.id as ProductId,
		p.name_en as ProductNameEn, 
		p.name_ar As ProductNameAr,
		-- ProductItem
		sp.product_item_id As ProductItemId,
		pi.sku As Sku,
		-- Seller Product
		sp.id as SellerProductId,
		sp.price As Price,
		sp.stock_quantity As StockQuantity,
        sp.is_active as IsActive,
		sp.created_at As CreatedAt,
		sp.updated_at As UpdatedAt,
		(
			SELECT
				spi.id AS Id,
				spi.seller_product_id as SellerProductId,
				spi.image_url AS ImageUrl,
				spi.created_at AS CreatedAt,
				spi.updated_at AS UpdatedAt
			FROM SellerProductImages spi
			WHERE spi.seller_product_id = @id
			FOR JSON PATH
        ) AS SellerProductImages

	FROM SellerProducts sp
	JOIN ProductItems pi on pi.id = sp.product_item_id
	Join Products p on p.id = pi.product_id
    WHERE sp.id = @Id
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
SELECT @json AS FullJson;

";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        await connection.OpenAsync();
                        using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);
                        {
                            if (!await reader.ReadAsync())
                                return new Result<SellerProductResponseForEdit>(false, "Product not found", null, 404);

                            // Read the entire JSON as a string first
                            string json = await reader.GetFieldValueAsync<string>(0);

                            var product = JsonSerializer.Deserialize<SellerProductResponseForEdit>(
                                json,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                            );

                            return new Result<SellerProductResponseForEdit>(true, "Product fetched successfully", product, 200);

                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error fetching product by id {id}: {ex}");
                        return new Result<SellerProductResponseForEdit>(false, "Error fetching product", null, 500);
                    }
                }
            }
        }

    }
}
