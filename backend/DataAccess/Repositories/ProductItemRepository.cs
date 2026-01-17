using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.DTOs.ProductItem;
using Jannara_Ecommerce.DTOs.VariationOption;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class ProductItemRepository : IProductItemRepository
    {

        private readonly string _connectionString;
        private readonly ILogger<IProductItemRepository> _logger;
        public ProductItemRepository(IOptions<DatabaseSettings> options, ILogger<IProductItemRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }

        public async Task<Result<ProductItemDTO>> AddNewAsync(int productId,
            string sku,
            SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"

INSERT INTO ProductItems (product_id, sku)
OUTPUT inserted.*
VALUES (@ProductId, @Sku);
";

            using var command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@ProductId", productId);
            command.Parameters.AddWithValue("@Sku", sku);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var insertedProduct = new ProductItemDTO
                (
                    reader.GetInt32(reader.GetOrdinal("id")),
                    reader.GetInt32(reader.GetOrdinal("product_id")),
                    reader.GetString(reader.GetOrdinal("Sku")),
                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                );
                return new Result<ProductItemDTO>(true, "product_item_added_successfully", insertedProduct);
            }

            return new Result<ProductItemDTO>(false, "failed_to_add_variation_product_item", null, 500);
        }


        public async Task<Result<IEnumerable<ProductItemDropdown>>> GetProductDropdownAsync(ProductItemDropdownRequest request)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
SELECT TOP 20
  id,
  sku
FROM ProductItems
WHERE 
product_id = @ProductId 
AND (
@SearchTerm IS NULL
OR @SearchTerm = ''
OR sku LIKE '%' + @SearchTerm + '%'
);
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchTerm", request.SearchTerm ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ProductId", request.ProductId);

                    var products = new List<ProductItemDropdown>();
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                products.Add(new ProductItemDropdown
                                (
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetString(reader.GetOrdinal("sku"))
                                ));
                            }
                            if (products.Count > 0)
                            {
                                return new Result<IEnumerable<ProductItemDropdown>>(true, "roles_retrieved_successfully", products);
                            }
                            return new Result<IEnumerable<ProductItemDropdown>>(false, "roles_not_found", null, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve all roles from database");
                        return new Result<IEnumerable<ProductItemDropdown>>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }
    
    }
}
