using Azure;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Data;
using System.Diagnostics.Metrics;

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
//                string query = @"select count(*) as total from products;
//select * from products
//order by id
//OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY ";

//                string query = @"select count(*) as total from products;
//SELECT
//    p.id,
//    p.name_ar,
//	p.name_en,
//	p.default_image_url,
//    MIN(sp.price) AS min_price
//FROM products p
//LEFT JOIN ProductItems pi
//    ON pi.product_id = p.id
//LEFT JOIN SellerProducts sp
//    ON sp.product_item_id = pi.id
//GROUP BY
//    p.id,
//    p.name_ar,
//	p.name_en,
//	p.default_image_url
//ORDER BY
//    p.id
//OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;";
                
//                string query = @"select count(*) as total from products;
//SELECT
//    p.id,
//    p.name_ar,
//    p.name_en,
//    p.default_image_url,
//    MIN(sp.price) AS min_price,
//    CASE 
//        WHEN cw.product_id IS NULL THEN 0 
//        ELSE 1 
//    END AS is_favorite
//FROM products p
//LEFT JOIN ProductItems pi
//    ON pi.product_id = p.id
//LEFT JOIN SellerProducts sp
//    ON sp.product_item_id = pi.id
//LEFT JOIN CustomerWishlist cw
//    ON cw.product_id = p.id
//    AND cw.customer_id = @customerId
//GROUP BY
//    p.id,
//    p.name_ar,
//    p.name_en,
//    p.default_image_url,
//    cw.product_id
//ORDER BY
//    p.id
//OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;";

//                string query = @"select count(*) as total from products;

//SELECT
//    p.id,
//    p.name_ar,
//    p.name_en,
//    p.default_image_url,
//    MIN(sp.price) AS min_price,
//    CAST(
//    CASE WHEN cw.product_id IS NULL THEN 0 ELSE 1 END
//    AS BIT
//	) AS is_favorite,
//    AVG(pr.rating * 1.0) AS  average_rating, 
//    COUNT(pr.rating) AS rating_count    
//FROM Products p
//LEFT JOIN ProductItems pi
//    ON pi.product_id = p.id
//LEFT JOIN SellerProducts sp
//    ON sp.product_item_id = pi.id
//LEFT JOIN CustomerWishlist cw
//    ON cw.product_id = p.id
//    AND cw.customer_id = @customerId
//LEFT JOIN ProductRatings pr
//    ON pr.product_id = p.id
//GROUP BY
//    p.id,
//    p.name_ar,
//    p.name_en,
//    p.default_image_url,
//    cw.product_id
//ORDER BY
//    p.id
//OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;";

//                string query = @"select count(*) as total from products p WHERE
//    (@CategoryId IS NULL OR p.category_id = @CategoryId)
//and
//(
//    @SearchTerm IS NULL
//    OR @SearchTerm = ''
//    OR p.name_en LIKE '%' + @SearchTerm + '%'
//    OR p.name_ar LIKE '%' + @SearchTerm + '%'
//    OR p.description_en LIKE '%' + @SearchTerm + '%'
//    OR p.description_ar LIKE '%' + @SearchTerm + '%'
//);

//SELECT
//    p.id,
//    p.name_ar,
//    p.name_en,
//    p.default_image_url,
//    p.created_at,
//    MIN(sp.price) AS min_price,
//    CAST(
//    CASE WHEN cw.product_id IS NULL THEN 0 ELSE 1 END
//    AS BIT
//	) AS is_favorite,
//    AVG(pr.rating * 1.0) AS  average_rating, 
//    COUNT(pr.rating) AS rating_count    
//FROM Products p
//LEFT JOIN ProductItems pi
//    ON pi.product_id = p.id
//LEFT JOIN SellerProducts sp
//    ON sp.product_item_id = pi.id
//LEFT JOIN CustomerWishlist cw
//    ON cw.product_id = p.id
//    AND cw.customer_id = @customerId
//LEFT JOIN ProductRatings pr
//    ON pr.product_id = p.id
//WHERE
//    (@CategoryId IS NULL OR p.category_id = @CategoryId)
//and
//(
//    @SearchTerm IS NULL
//    OR @SearchTerm = ''
//    OR p.name_en LIKE '%' + @SearchTerm + '%'
//    OR p.name_ar LIKE '%' + @SearchTerm + '%'
//    OR p.description_en LIKE '%' + @SearchTerm + '%'
//    OR p.description_ar LIKE '%' + @SearchTerm + '%'
//)

//GROUP BY
//    p.id,
//    p.name_ar,
//    p.name_en,
//    p.default_image_url,
//    p.created_at,
//    cw.product_id
//ORDER BY
//    CASE 
//        WHEN @SortBy = 'price_asc' AND MIN(sp.price) IS NULL THEN 1
//        ELSE 0
//    END,
//    CASE 
//        WHEN @SortBy = 'price_asc' THEN MIN(sp.price)
//    END ASC,
//    CASE 
//        WHEN @SortBy = 'price_desc' AND MIN(sp.price) IS NULL THEN 1
//        ELSE 0
//    END,
//    CASE 
//        WHEN @SortBy = 'price_desc' THEN MIN(sp.price)
//    END DESC,
//    CASE WHEN @SortBy = 'newest' THEN p.id END DESC,
//    CASE WHEN @SortBy = 'oldest' THEN p.id END ASC,
//    p.id
//OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;";

                string query = @"

SELECT
    p.id,
    p.name_ar,
    p.name_en,
    p.default_image_url,
    p.created_at,
    MIN(sp.price) AS min_price,
    CAST(CASE WHEN MAX(cw.product_id) IS NULL THEN 0 ELSE 1 END AS BIT) AS is_favorite,
    AVG(pr.rating * 1.0) AS  average_rating, 
    COUNT(pr.rating) AS rating_count    
FROM Products p
LEFT JOIN ProductItems pi
    ON pi.product_id = p.id
LEFT JOIN SellerProducts sp
    ON sp.product_item_id = pi.id
{WISHLIST_JOIN}
LEFT JOIN ProductRatings pr
    ON pr.product_id = p.id
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
                                    reader.GetString(reader.GetOrdinal("default_image_url")),
                                    reader.GetString(reader.GetOrdinal("name_en")),
                                    reader.GetString(reader.GetOrdinal("name_ar")),
                                    reader.IsDBNull(reader.GetOrdinal("min_price")) ? null : reader.GetDecimal(reader.GetOrdinal("min_price")),
                                    reader.GetBoolean(reader.GetOrdinal("is_favorite")),
                                    reader.IsDBNull(reader.GetOrdinal("average_rating")) ? null : reader.GetDouble(reader.GetOrdinal("average_rating")),
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

    }
}
