using Azure;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

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

        public async Task<Result<PagedResponseDTO<ProductResponseDTO>>> GetAllAsync(int pageNumber = 1, int pageSize = 20, int customerId = -1)
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
                
                string query = @"select count(*) as total from products;
SELECT
    p.id,
    p.name_ar,
    p.name_en,
    p.default_image_url,
    MIN(sp.price) AS min_price,
    CASE 
        WHEN cw.product_id IS NULL THEN 0 
        ELSE 1 
    END AS is_favorite
FROM products p
LEFT JOIN ProductItems pi
    ON pi.product_id = p.id
LEFT JOIN SellerProducts sp
    ON sp.product_item_id = pi.id
LEFT JOIN CustomerWishlist cw
    ON cw.product_id = p.id
    AND cw.customer_id = @customerId
GROUP BY
    p.id,
    p.name_ar,
    p.name_en,
    p.default_image_url,
    cw.product_id
ORDER BY
    p.id
OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;";

                using (var command = new SqlCommand(query, connection))
                {
                    int offset = (pageNumber - 1) * pageSize;
                    command.Parameters.AddWithValue("@offset", offset);
                    command.Parameters.AddWithValue("@pageSize", pageSize);
                    command.Parameters.AddWithValue("@customerId", customerId);
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
                            await reader.NextResultAsync();
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
                                    false
                                ));
                            }
                            if (products.Count() < 1)
                            {
                                return new Result<PagedResponseDTO<ProductResponseDTO>>(false, "products_not_found", null, 404);
                            }
                            var response = new PagedResponseDTO<ProductResponseDTO>(total, pageNumber, pageSize, products);
                            return new Result<PagedResponseDTO<ProductResponseDTO>>(true, "products_retrieved_successfully", response, 200);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve products for page {PageNumber} with page size {PageSize}", pageNumber, pageSize);
                        return new Result<PagedResponseDTO<ProductResponseDTO>>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }

    }
}
