using Jannara_Ecommerce.DataAccess.Interfaces;
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

    }
}
