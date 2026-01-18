using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.ProductItemImage;
using Jannara_Ecommerce.DTOs.SellerProductImage;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class SellerProductImageRepository : ISellerProductImageRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<IProductItemImageRepository> _logger;
        public SellerProductImageRepository(IOptions<DatabaseSettings> options, ILogger<IProductItemImageRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }


        public async Task<Result<SellerProductImageDTO>> AddNewAsync(
    SellerProductImageCreateDBDTO productImage,
    SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"


INSERT INTO [dbo].[SellerProductImages]
(
    [seller_product_id],
    [image_url]
)
OUTPUT inserted.*
VALUES
(
    @SellerProductId,
    @ImageUrl
);

";

            using var command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@SellerProductId", productImage.SellerProductId);
            command.Parameters.AddWithValue("@ImageUrl", productImage.ImageUrl);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var insertedProduct = new SellerProductImageDTO
                (
                    reader.GetInt32(reader.GetOrdinal("id")),
                    reader.GetInt32(reader.GetOrdinal("seller_product_id")),
                    reader.GetString(reader.GetOrdinal("image_url")),
                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                );
                return new Result<SellerProductImageDTO>(true, "product_item_image_added_successfully", insertedProduct);
            }

            return new Result<SellerProductImageDTO>(false, "failed_to_add_product_item_image", null, 500);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM SellerProductImages WHERE id = @id";
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

    }
}
