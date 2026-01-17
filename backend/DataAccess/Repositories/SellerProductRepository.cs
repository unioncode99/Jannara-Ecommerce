using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.DTOs.SellerProduct;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Utilities.Zlib;

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

    }
}
