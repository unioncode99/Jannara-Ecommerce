using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.ProductItemImage;
using Jannara_Ecommerce.DTOs.ProductItemVariationOption;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class ProductItemVariationOptionRepository : IProductItemVariationOptionRepository
    {

        private readonly string _connectionString;
        private readonly ILogger<IProductItemVariationOptionRepository> _logger;
        public ProductItemVariationOptionRepository(IOptions<DatabaseSettings> options, ILogger<IProductItemVariationOptionRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }

        public async Task<Result<ProductItemVariationOptionDTO>> AddNewAsync(int productItemId,
            int variationOptionId,
            SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"

INSERT INTO ProductItemVariationOptions
(product_item_id, variation_option_id)
OUTPUT inserted.*
VALUES (@ProductItemId, @VariationOptionId);

";

            using var command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@ProductItemId", productItemId);
            command.Parameters.AddWithValue("@VariationOptionId", variationOptionId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var insertedProduct = new ProductItemVariationOptionDTO
                (
                    reader.GetInt32(reader.GetOrdinal("id")),
                    reader.GetInt32(reader.GetOrdinal("variation_option_id")),
                    reader.GetInt32(reader.GetOrdinal("product_item_id")),
                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                );
                return new Result<ProductItemVariationOptionDTO>(true, "product_item_image_added_successfully", insertedProduct);
            }

            return new Result<ProductItemVariationOptionDTO>(false, "failed_to_add_product_item_image", null, 500);
        }

    }
}
