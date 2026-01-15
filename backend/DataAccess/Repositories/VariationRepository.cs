using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.DTOs.Role;
using Jannara_Ecommerce.DTOs.Variation;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class VariationRepository : IVariationRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<IVariationRepository> _logger;
        public VariationRepository(IOptions<DatabaseSettings> options, ILogger<IVariationRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }

        public async Task<Result<VariationDTO>> AddNewAsync(int productId,
            VariationCreateDTO variation, 
            SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"

INSERT INTO Variations (product_id, name_en, name_ar)
OUTPUT inserted.*
VALUES (@ProductId, @NameEn, @NameAr);
";

            using var command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@ProductId", productId);
            command.Parameters.AddWithValue("@NameEn", variation.NameEn);
            command.Parameters.AddWithValue("@NameAr", variation.NameAr);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var insertedProduct = new VariationDTO
                (
                    reader.GetInt32(reader.GetOrdinal("id")),
                    reader.GetInt32(reader.GetOrdinal("product_id")),
                    reader.GetString(reader.GetOrdinal("name_en")),
                    reader.GetString(reader.GetOrdinal("name_ar")),
                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                );
                return new Result<VariationDTO>(true, "variation_added_successfully", insertedProduct);
            }

            return new Result<VariationDTO>(false, "failed_to_add_variation", null, 500);
        }

        public async Task<Result<VariationDTO>> AddNewAsync(VariationCreateOneDTO variationCreateOneDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO Variations (product_id, name_en, name_ar)
OUTPUT inserted.*
VALUES (@ProductId, @NameEn, @NameAr);
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", variationCreateOneDTO.ProductId);
                    command.Parameters.AddWithValue("@NameEn", variationCreateOneDTO.NameEn);
                    command.Parameters.AddWithValue("@NameAr", variationCreateOneDTO.NameAr);
                    try
                    {
                        await connection.OpenAsync();
                        using var reader = await command.ExecuteReaderAsync();
                        if (await reader.ReadAsync())
                        {
                            var insertedProduct = new VariationDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetInt32(reader.GetOrdinal("product_id")),
                                reader.GetString(reader.GetOrdinal("name_en")),
                                reader.GetString(reader.GetOrdinal("name_ar")),
                                reader.GetDateTime(reader.GetOrdinal("created_at")),
                                reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            );
                            return new Result<VariationDTO>(true, "variation_added_successfully", insertedProduct);
                        }

                        return new Result<VariationDTO>(false, "failed_to_add_variation", null, 500);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to add a new role with NameEn {NameEn} and NameAr {NameAr}", variationCreateOneDTO.NameEn, variationCreateOneDTO.NameAr);
                        return new Result<VariationDTO>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Variations WHERE id = @id";
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

        public async Task<Result<VariationDTO>> UpdateAsync(int id, VariationUpdateDTO variationUpdateDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE Variations
SET 
    name_en = @NameEn,
    name_ar = @NameAr
WHERE id = @id;
select 
*
from
Variations
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@NameEn", variationUpdateDTO.NameEn);
                    command.Parameters.AddWithValue("@NameAr", variationUpdateDTO.NameAr);
                    try
                    {
                        await connection.OpenAsync();
                        using var reader = await command.ExecuteReaderAsync();
                        if (await reader.ReadAsync())
                        {
                            var insertedProduct = new VariationDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetInt32(reader.GetOrdinal("product_id")),
                                reader.GetString(reader.GetOrdinal("name_en")),
                                reader.GetString(reader.GetOrdinal("name_ar")),
                                reader.GetDateTime(reader.GetOrdinal("created_at")),
                                reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            );
                            return new Result<VariationDTO>(true, "variation_added_successfully", insertedProduct);
                        }

                        return new Result<VariationDTO>(false, "failed_to_add_variation", null, 500);
                    }
                    catch (SqlException ex)
                    {
                        _logger.LogError(ex, "Failed to update role with RoleId {RoleId}", id);
                        return new Result<VariationDTO>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }
    
    }
}
