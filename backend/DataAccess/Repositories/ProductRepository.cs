using Azure;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.DTOs.Role;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.DTOs.Variation;
using Jannara_Ecommerce.DTOs.VariationOption;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Data;
using System.Diagnostics.Metrics;
using System.Text.Json;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ISellerRepository> _logger;
        public ProductRepository(IOptions<DatabaseSettings> options, ILogger<ISellerRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }

        public async Task<Result<PagedResponseDTO<ProductResponseDTO>>> GetAllAsync(FilterProductDTO filter)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                string query = @"

SELECT
    p.id,
    p.public_id,
    p.name_ar,
    p.name_en,
    p.default_image_url,
    p.created_at,
    MIN(sp.price) AS min_price,
    CAST(CASE WHEN MAX(cw.product_id) IS NULL THEN 0 ELSE 1 END AS BIT) AS is_favorite,
    ISNULL(MAX(pr.average_rating), 0) AS average_rating,
    ISNULL(MAX(pr.rating_count), 0) AS rating_count
FROM Products p
LEFT JOIN ProductItems pi
    ON pi.product_id = p.id
LEFT JOIN SellerProducts sp
    ON sp.product_item_id = pi.id
{WISHLIST_JOIN}
LEFT JOIN (
    SELECT
        product_id,
        COUNT(*) AS rating_count,
        AVG(rating * 1.0) AS average_rating
    FROM ProductRatings
    GROUP BY product_id
) pr ON pr.product_id = p.id
WHERE
    (@CategoryId IS NULL OR p.category_id = @CategoryId)
and
(
    @SearchTerm IS NULL
    OR @SearchTerm = ''
    OR p.name_en LIKE '%' + @SearchTerm + '%'
    OR p.name_ar LIKE '%' + @SearchTerm + '%'
    OR p.description_en LIKE '%' + @SearchTerm + '%'
    OR p.description_ar LIKE '%' + @SearchTerm + '%'
)

GROUP BY
    p.id,
    p.public_id,
    p.name_ar,
    p.name_en,
    p.default_image_url,
    p.created_at
ORDER BY
    CASE 
        WHEN @SortBy = 'price_asc' AND MIN(sp.price) IS NULL THEN 1
        ELSE 0
    END,
    CASE 
        WHEN @SortBy = 'price_asc' THEN MIN(sp.price)
    END ASC,
    CASE 
        WHEN @SortBy = 'price_desc' AND MIN(sp.price) IS NULL THEN 1
        ELSE 0
    END,
    CASE 
        WHEN @SortBy = 'price_desc' THEN MIN(sp.price)
    END DESC,
    CASE WHEN @SortBy = 'newest' THEN p.id END DESC,
    CASE WHEN @SortBy = 'oldest' THEN p.id END ASC,
    p.id
OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;";

                string wishlistJoin = filter.IsFavoritesOnly == true
        ? "INNER JOIN CustomerWishlist cw ON cw.product_id = p.id AND cw.customer_id = @customerId"
        : "LEFT JOIN CustomerWishlist cw ON cw.product_id = p.id AND cw.customer_id = @customerId";

                string countQuery = filter.IsFavoritesOnly == true
    ? @"SELECT COUNT(DISTINCT p.id) as total
        FROM Products p
        INNER JOIN CustomerWishlist cw ON cw.product_id = p.id AND cw.customer_id = @customerId
        WHERE (@CategoryId IS NULL OR p.category_id = @CategoryId)
        AND (@SearchTerm IS NULL OR @SearchTerm = ''
            OR p.name_en LIKE '%' + @SearchTerm + '%'
            OR p.name_ar LIKE '%' + @SearchTerm + '%'
            OR p.description_en LIKE '%' + @SearchTerm + '%'
            OR p.description_ar LIKE '%' + @SearchTerm + '%');"
    : @"SELECT COUNT(*) as total
        FROM products p
        WHERE (@CategoryId IS NULL OR p.category_id = @CategoryId)
        AND (@SearchTerm IS NULL OR @SearchTerm = ''
            OR p.name_en LIKE '%' + @SearchTerm + '%'
            OR p.name_ar LIKE '%' + @SearchTerm + '%'
            OR p.description_en LIKE '%' + @SearchTerm + '%'
            OR p.description_ar LIKE '%' + @SearchTerm + '%');";


                query = query.Replace("{WISHLIST_JOIN}", wishlistJoin);

                query = countQuery + query;


                using (var command = new SqlCommand(query, connection))
                {
                    int offset = (filter.PageNumber - 1) * filter.PageSize;
                    command.Parameters.AddWithValue("@offset", offset);
                    command.Parameters.AddWithValue("@pageSize", filter.PageSize);
                    command.Parameters.AddWithValue("@customerId", filter.CustomerId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@SortBy", filter.SortBy ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CategoryId", filter.CategoryId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@SearchTerm", filter.SearchTerm ?? (object)DBNull.Value);
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            int total = 0;
                            if (await reader.ReadAsync())
                            {
                                total = reader.GetInt32(reader.GetOrdinal("total"));
                            }
                            //await reader.NextResultAsync();
                            if (!await reader.NextResultAsync())
                            {
                                return new Result<PagedResponseDTO<ProductResponseDTO>>(false, "products_not_found", null, 404);
                            }
                            if (!reader.HasRows)
                            {
                                return new Result<PagedResponseDTO<ProductResponseDTO>>(false, "products_not_found", null, 404);
                            }
                            var products = new List<ProductResponseDTO>();
                            while (await reader.ReadAsync())
                            {
                                // for test
                                //products.Add(new ProductResponseDTO
                                //(
                                //    reader.GetInt32(reader.GetOrdinal("id")),
                                //    reader.GetString(reader.GetOrdinal("default_image_url")),
                                //    reader.GetString(reader.GetOrdinal("name_en")),
                                //    reader.GetString(reader.GetOrdinal("name_ar")),
                                //    1000,
                                //    false
                                //));
                                // reader.IsDBNull(reader.GetOrdinal("website_url")) ? null : reader.GetString(reader.GetOrdinal("website_url"))
                                products.Add(new ProductResponseDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetGuid(reader.GetOrdinal("public_id")),
                                    reader.GetString(reader.GetOrdinal("default_image_url")),
                                    reader.GetString(reader.GetOrdinal("name_en")),
                                    reader.GetString(reader.GetOrdinal("name_ar")),
                                    reader.IsDBNull(reader.GetOrdinal("min_price")) ? null : reader.GetDecimal(reader.GetOrdinal("min_price")),
                                    reader.GetBoolean(reader.GetOrdinal("is_favorite")),
                                    reader.IsDBNull(reader.GetOrdinal("average_rating")) ? null : reader.GetDecimal(reader.GetOrdinal("average_rating")),
                                    reader.GetInt32(reader.GetOrdinal("rating_count"))
                                ));
                            }
                            if (products.Count() < 1)
                            {
                                return new Result<PagedResponseDTO<ProductResponseDTO>>(false, "products_not_found", null, 404);
                            }
                            var response = new PagedResponseDTO<ProductResponseDTO>(total, filter.PageNumber, filter.PageSize, products);
                            return new Result<PagedResponseDTO<ProductResponseDTO>>(true, "products_retrieved_successfully", response, 200);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve products for page {PageNumber} with page size {PageSize}", filter.PageNumber, filter.PageSize);
                        return new Result<PagedResponseDTO<ProductResponseDTO>>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }

        public async Task<Result<ProductDetailDTO>> GetByPublicIdAsync(Guid publicId, int? customerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
DECLARE @json NVARCHAR(MAX);

SELECT @json = (
    SELECT
        p.id AS ProductId,
        p.public_id AS PublicId,
        p.default_image_url AS DefaultImageUrl,
        p.name_en AS NameEn,
        p.name_ar AS NameAr,
        p.description_en AS DescriptionEn,
        p.description_ar AS DescriptionAr,
        p.weight_kg AS WeightKg,

        p.created_at AS CreatedAt,
        p.updated_at AS UpdatedAt,

		-- MinPrice
(
    SELECT MIN(sp.price)
    FROM ProductItems pi2
    JOIN SellerProducts sp ON sp.product_item_id = pi2.id
    WHERE pi2.product_id = p.id
      AND sp.is_active = 1
) AS MinPrice,

		-- IsFavorite
				        CASE WHEN EXISTS (
            SELECT 1 
            FROM CustomerWishlist cw 
            WHERE cw.product_id = p.id AND cw.customer_id = @customerId
        ) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsFavorite,
        -- AverageRating
        (SELECT AVG(CAST(r.rating AS FLOAT)) 
         FROM ProductRatings r 
         WHERE r.product_id = p.id) AS AverageRating,

		 -- RatingCount
         
        (SELECT COUNT(*) 
         FROM ProductRatings r 
         WHERE r.product_id = p.id) AS RatingCount,

		 -- Brand
JSON_QUERY(
(
    SELECT 
        b.id AS Id,
        b.name_en AS NameEn,
        b.name_ar AS NameAr,
        b.logo_url AS LogoUrl,
        b.website_url AS WebsiteUrl,
        b.description_en AS DescriptionEn,
        b.description_ar AS DescriptionAr,
        b.created_at AS CreatedAt,
        b.updated_at AS UpdatedAt
    FROM Brands b
    WHERE b.id = p.brand_id
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
)
) AS Brand,
		 
        (
            SELECT
                v.id AS VariationId,
                v.name_en AS NameEn,
                v.name_ar AS NameAr,
                v.created_at AS CreatedAt,
                v.updated_at AS UpdatedAt,
                (
                    SELECT
                        vo.id AS VariationOptionId,
                        vo.value_en AS ValueEn,
                        vo.value_ar AS ValueAr,
                        vo.created_at AS CreatedAt,
                        vo.updated_at AS UpdatedAt
                    FROM VariationOptions vo
                    WHERE vo.variation_id = v.id
                    FOR JSON PATH
                ) AS Options
            FROM Variations v
            WHERE v.product_id = p.id
            FOR JSON PATH
        ) AS Variations,
        (
            SELECT
                pi.id AS ProductItemId,
                pi.sku AS Sku,
                pi.created_at AS CreatedAt,
                pi.updated_at AS UpdatedAt,
                (
                    SELECT
                        pivo.id AS Id,
                        pivo.variation_option_id AS variationOptionId,
                        pivo.created_at AS CreatedAt,
                        pivo.updated_at AS UpdatedAt
                    FROM ProductItemVariationOptions pivo
                    WHERE pivo.product_item_id = pi.id
                    FOR JSON PATH
                ) AS ProductItemVariationOptions,
                (
                    SELECT
                        pii.id AS ProductItemImageId,
                        pii.image_url AS ImageUrl,
                        pii.is_primary AS IsPrimary,
                        pii.created_at AS CreatedAt,
                        pii.updated_at AS UpdatedAt
                    FROM ProductItemImages pii
                    WHERE pii.product_item_id = pi.id
                    FOR JSON PATH
                ) AS ProductItemImages,
                (
                    SELECT
                        sp.id AS SellerProductId,
                        sp.seller_id AS SellerId,
                        sp.price AS Price,
                        sp.stock_quantity AS StockQuantity,
                        sp.is_active AS IsActive,
                        sp.created_at AS CreatedAt,
                        sp.updated_at AS UpdatedAt,
                        (
                            SELECT
                                spi.id AS SellerProductImageId,
                                spi.image_url AS ImageUrl,
                                spi.created_at AS CreatedAt,
                                spi.updated_at AS UpdatedAt
                            FROM SellerProductImages spi
                            WHERE spi.seller_product_id = sp.id
                            FOR JSON PATH
                        ) AS SellerProductImages
                    FROM SellerProducts sp
                    WHERE sp.product_item_id = pi.id
                    FOR JSON PATH
                ) AS SellerProducts
            FROM ProductItems pi
            WHERE pi.product_id = p.id
            FOR JSON PATH
        ) AS ProductItems
    FROM Products p
    WHERE p.public_id = @publicId
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
);

SELECT @json AS FullJson;
";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@publicId", publicId);
                    command.Parameters.AddWithValue("@customerId", customerId ?? (object)DBNull.Value);
                    try
                    {
                        await connection.OpenAsync();
                        using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);
                        {
                            if (!await reader.ReadAsync())
                                return new Result<ProductDetailDTO>(false, "Product not found", null, 404);

                            // Read the entire JSON as a string first
                            string json = await reader.GetFieldValueAsync<string>(0);

                            var product = JsonSerializer.Deserialize<ProductDetailDTO>(
                                json,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                            );

                            return new Result<ProductDetailDTO>(true, "Product fetched successfully", product, 200);

                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error fetching product by id {publicId}: {ex}");
                        return new Result<ProductDetailDTO>(false, "Error fetching product", null, 500);
                    }
                }
            }
        }

        public async Task<Result<ProductDTO>> AddNewAsync(ProductCreateDBDTO product, SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"
INSERT INTO Products
(brand_id, category_id, default_image_url, name_en, name_ar,
 description_en, description_ar, weight_kg)
OUTPUT inserted.*
VALUES
(@BrandId, @CategoryId, @DefaultImageUrl, @NameEn, @NameAr,
 @DescriptionEn, @DescriptionAr, @WeightKg);
";

            using var command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@BrandId", (object?)product.BrandId ?? DBNull.Value);
            command.Parameters.AddWithValue("@CategoryId", (object?)product.CategoryId ?? DBNull.Value);
            command.Parameters.AddWithValue("@DefaultImageUrl", (object?)product.DefaultImageUrl ?? DBNull.Value);
            command.Parameters.AddWithValue("@NameEn", product.NameEn);
            command.Parameters.AddWithValue("@NameAr", product.NameAr);
            command.Parameters.AddWithValue("@DescriptionEn", (object?)product.DescriptionEn ?? DBNull.Value);
            command.Parameters.AddWithValue("@DescriptionAr", (object?)product.DescriptionAr ?? DBNull.Value);
            command.Parameters.AddWithValue("@WeightKg", product.WeightKg);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var insertedProduct = new ProductDTO
                (
                    reader.GetInt32(reader.GetOrdinal("id")),
                    reader.GetGuid(reader.GetOrdinal("public_id")),
                    reader.GetInt32(reader.GetOrdinal("category_id")),
                    reader.IsDBNull(reader.GetOrdinal("brand_id")) ? default : reader.GetInt32(reader.GetOrdinal("id")),
                    reader.GetString(reader.GetOrdinal("default_image_url")),
                    reader.GetString(reader.GetOrdinal("name_en")),
                    reader.GetString(reader.GetOrdinal("name_ar")),
                    reader.GetString(reader.GetOrdinal("description_en")),
                    reader.GetString(reader.GetOrdinal("description_ar")),
                    reader.GetDecimal(reader.GetOrdinal("weight_kg")),
                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                );
                return new Result<ProductDTO>(true, "product_added_successfully", insertedProduct);
            }

            return new Result<ProductDTO>(false, "failed_to_add_product", null, 500);
        }


        public async Task<Result<PagedResponseDTO<ProductGeneralResponseDTO>>> GetAllGeneralAsync(GeneralProductFilterDTO filter)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"

-- TOTAL COUNT
SELECT COUNT(p.id) AS total
FROM products p
LEFT JOIN brands b ON p.brand_id = b.id
LEFT JOIN ProductCategories c ON p.category_id = c.id
WHERE
    (@CategoryId IS NULL OR p.category_id = @CategoryId)
AND (@BrandId IS NULL OR p.brand_id = @BrandId)
AND (
        @SearchTerm IS NULL
        OR p.name_en LIKE '%' + @SearchTerm + '%'
        OR p.name_ar LIKE '%' + @SearchTerm + '%'
        OR p.description_en LIKE '%' + @SearchTerm + '%'
        OR p.description_ar LIKE '%' + @SearchTerm + '%'
    );

-- PAGED DATA
SELECT
    p.id,
    p.public_id,
    p.category_id,
    p.brand_id,
    p.default_image_url,
    p.name_en,
    p.name_ar,
    p.description_en,
    p.description_ar,
    p.weight_kg,
    p.created_at,
    p.updated_at,
    c.name_en AS category_name_en,
    c.name_ar AS category_name_ar,
    b.name_en AS brand_name_en,
    b.name_ar AS brand_name_ar
FROM products p
LEFT JOIN brands b ON p.brand_id = b.id
LEFT JOIN ProductCategories c ON p.category_id = c.id
WHERE
    (@CategoryId IS NULL OR p.category_id = @CategoryId)
AND (@BrandId IS NULL OR p.brand_id = @BrandId)
AND (
        @SearchTerm IS NULL
        OR p.name_en LIKE '%' + @SearchTerm + '%'
        OR p.name_ar LIKE '%' + @SearchTerm + '%'
        OR p.description_en LIKE '%' + @SearchTerm + '%'
        OR p.description_ar LIKE '%' + @SearchTerm + '%'
    )
ORDER BY
    CASE WHEN @SortBy = 'newest' THEN p.id END DESC,
    CASE WHEN @SortBy = 'oldest' THEN p.id END ASC,
    p.id DESC
OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;
";

            using var command = new SqlCommand(query, connection);

            int offset = (filter.PageNumber - 1) * filter.PageSize;
            command.Parameters.AddWithValue("@offset", offset);
            command.Parameters.AddWithValue("@pageSize", filter.PageSize);
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
                    return new Result<PagedResponseDTO<ProductGeneralResponseDTO>>(false, "products_not_found", null, 404);
                }

                var products = new List<ProductGeneralResponseDTO>();
                while (await reader.ReadAsync())
                {
                    products.Add(new ProductGeneralResponseDTO
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        PublicId = reader.GetGuid(reader.GetOrdinal("public_id")),
                        CategoryId = reader.GetInt32(reader.GetOrdinal("category_id")),
                        BrandId = reader.IsDBNull(reader.GetOrdinal("brand_id"))
                                  ? null
                                  : reader.GetInt32(reader.GetOrdinal("brand_id")),
                        DefaultImageUrl = reader.GetString(reader.GetOrdinal("default_image_url")),
                        NameEn = reader.GetString(reader.GetOrdinal("name_en")),
                        NameAr = reader.GetString(reader.GetOrdinal("name_ar")),
                        DescriptionEn = reader.IsDBNull(reader.GetOrdinal("description_en"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("description_en")),
                        DescriptionAr = reader.IsDBNull(reader.GetOrdinal("description_ar"))
                                        ? null
                                        : reader.GetString(reader.GetOrdinal("description_ar")),
                        WeightKg = reader.GetDecimal(reader.GetOrdinal("weight_kg")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                        UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at")),
                        CategoryNameEn = reader.IsDBNull(reader.GetOrdinal("category_name_en"))
                                         ? null
                                         : reader.GetString(reader.GetOrdinal("category_name_en")),
                        CategoryNameAr = reader.IsDBNull(reader.GetOrdinal("category_name_ar"))
                                         ? null
                                         : reader.GetString(reader.GetOrdinal("category_name_ar")),
                        BrandNameEn = reader.IsDBNull(reader.GetOrdinal("brand_name_en"))
                                      ? null
                                      : reader.GetString(reader.GetOrdinal("brand_name_en")),
                        BrandNameAr = reader.IsDBNull(reader.GetOrdinal("brand_name_ar"))
                                      ? null
                                      : reader.GetString(reader.GetOrdinal("brand_name_ar")),
                    });
                }

                var response = new PagedResponseDTO<ProductGeneralResponseDTO>(total, filter.PageNumber, filter.PageSize, products);
                return new Result<PagedResponseDTO<ProductGeneralResponseDTO>>(true, "products_retrieved_successfully", response, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve products for page {PageNumber} with page size {PageSize}", filter.PageNumber, filter.PageSize);
                return new Result<PagedResponseDTO<ProductGeneralResponseDTO>>(false, "internal_server_error", null, 500);
            }
        }


    }
}
