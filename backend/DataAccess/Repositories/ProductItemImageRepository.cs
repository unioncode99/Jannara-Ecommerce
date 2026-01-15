using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.ProductItem;
using Jannara_Ecommerce.DTOs.ProductItemImage;
using Jannara_Ecommerce.DTOs.Variation;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class ProductItemImageRepository : IProductItemImageRepository
    {

        private readonly string _connectionString;
        private readonly ILogger<IProductItemImageRepository> _logger;
        public ProductItemImageRepository(IOptions<DatabaseSettings> options, ILogger<IProductItemImageRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }

        public async Task<Result<ProductItemImageDTO>> AddNewAsync(int productItemId,
            ProductItemImageCreateDBDTO productItemImage,
            SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"

INSERT INTO ProductItemImages (product_item_id, image_url, is_primary)
OUTPUT inserted.*
VALUES (@ProductItemId, @ImageUrl, @IsPrimary);
";

            using var command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@ProductItemId", productItemId);
            command.Parameters.AddWithValue("@ImageUrl", productItemImage.ImageUrl);
            command.Parameters.AddWithValue("@IsPrimary", productItemImage.IsPrimary);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var insertedProduct = new ProductItemImageDTO
                (
                    reader.GetInt32(reader.GetOrdinal("id")),
                    reader.GetInt32(reader.GetOrdinal("product_item_id")),
                    reader.GetString(reader.GetOrdinal("image_url")),
                    reader.GetBoolean(reader.GetOrdinal("is_primary")),
                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                );
                return new Result<ProductItemImageDTO>(true, "product_item_image_added_successfully", insertedProduct);
            }

            return new Result<ProductItemImageDTO>(false, "failed_to_add_product_item_image", null, 500);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM ProductItemImages WHERE id = @id";
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

        public async Task<Result<ProductItemImageDTO>> SetPrimaryAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE ProductItemImages
SET 
    is_primary = 0,
WHERE id <> @id;

UPDATE ProductItemImages
SET 
    is_primary = 1,
WHERE id = @id;

select 
*
from
ProductItemImages
where
id = @id;
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    try
                    {
                        await connection.OpenAsync();
                        using var reader = await command.ExecuteReaderAsync();
                        if (await reader.ReadAsync())
                        {
                            var insertedProduct = new ProductItemImageDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetInt32(reader.GetOrdinal("product_item_id")),
                                reader.GetString(reader.GetOrdinal("image_url")),
                                reader.GetBoolean(reader.GetOrdinal("is_primary")),
                                reader.GetDateTime(reader.GetOrdinal("created_at")),
                                reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            );
                            return new Result<ProductItemImageDTO>(true, "product_item_image_added_successfully", insertedProduct);
                        }

                        return new Result<ProductItemImageDTO>(false, "failed_to_add_product_item_image", null, 500);
                    }
                    catch (SqlException ex)
                    {
                        _logger.LogError(ex, "Failed to update role with RoleId {RoleId}", id);
                        return new Result<ProductItemImageDTO>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }
    }
}
