using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Variation;
using Jannara_Ecommerce.DTOs.VariationOption;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class VariationOptionRepository : IVariationOptionRepository
    {

        private readonly string _connectionString;
        private readonly ILogger<IVariationOptionRepository> _logger;
        public VariationOptionRepository(IOptions<DatabaseSettings> options, ILogger<IVariationOptionRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }

        public async Task<Result<VariationOptionDTO>> AddNewAsync(int variationId,
            VariationOptionCreateDTO variationOption,
            SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"

INSERT INTO VariationOptions (variation_id, value_en, value_ar)
OUTPUT inserted.*
VALUES (@VariationId, @ValueEn, @ValueAr);
";

            using var command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@VariationId", variationId);
            command.Parameters.AddWithValue("@ValueEn", variationOption.ValueEn);
            command.Parameters.AddWithValue("@ValueAr", variationOption.ValueAr);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var insertedProduct = new VariationOptionDTO
                (
                    reader.GetInt32(reader.GetOrdinal("id")),
                    reader.GetInt32(reader.GetOrdinal("variation_id")),
                    reader.GetString(reader.GetOrdinal("value_en")),
                    reader.GetString(reader.GetOrdinal("value_ar")),
                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                );
                return new Result<VariationOptionDTO>(true, "variation_option_added_successfully", insertedProduct);
            }

            return new Result<VariationOptionDTO>(false, "failed_to_add_variation_option", null, 500);
        }

        public async Task<Result<VariationOptionDTO>> AddNewAsync(VariationOptionCreateOneDTO variationOptionCreateOneDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO VariationOptions (variation_id, value_en, value_ar)
OUTPUT inserted.*
VALUES (@VariationId, @ValueEn, @ValueAr);
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@VariationId", variationOptionCreateOneDTO.VariationId);
                    command.Parameters.AddWithValue("@ValueEn", variationOptionCreateOneDTO.ValueEn);
                    command.Parameters.AddWithValue("@ValueAr", variationOptionCreateOneDTO.ValueAr);
                    try
                    {
                        await connection.OpenAsync();
                        using var reader = await command.ExecuteReaderAsync();
                        if (await reader.ReadAsync())
                        {
                            var insertedProduct = new VariationOptionDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetInt32(reader.GetOrdinal("variation_id")),
                                reader.GetString(reader.GetOrdinal("value_en")),
                                reader.GetString(reader.GetOrdinal("value_ar")),
                                reader.GetDateTime(reader.GetOrdinal("created_at")),
                                reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            );
                            return new Result<VariationOptionDTO>(true, "variation_option_added_successfully", insertedProduct);
                        }

                        return new Result<VariationOptionDTO>(false, "failed_to_add_variation_option", null, 500);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to add a new role");
                        return new Result<VariationOptionDTO>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM VariationOptions WHERE id = @id";
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

        public async Task<Result<VariationOptionDTO>> UpdateAsync(int id, VariationOptionUpdateDTO variationOptionUpdateDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE VariationOptions
SET 
    value_en = @ValueEn,
    value_ar = @ValueAr
WHERE id = @id;
select 
*
from
Variations
where
id = @id;
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@ValueEn", variationOptionUpdateDTO.ValueEn);
                    command.Parameters.AddWithValue("@ValueAr", variationOptionUpdateDTO.ValueAr);
                    try
                    {
                        await connection.OpenAsync();
                        using var reader = await command.ExecuteReaderAsync();
                        if (await reader.ReadAsync())
                        {
                            var insertedProduct = new VariationOptionDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetInt32(reader.GetOrdinal("variation_id")),
                                reader.GetString(reader.GetOrdinal("value_en")),
                                reader.GetString(reader.GetOrdinal("value_ar")),
                                reader.GetDateTime(reader.GetOrdinal("created_at")),
                                reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            );
                            return new Result<VariationOptionDTO>(true, "variation_option_added_successfully", insertedProduct);
                        }

                        return new Result<VariationOptionDTO>(false, "failed_to_add_variation_option", null, 500);
                    }
                    catch (SqlException ex)
                    {
                        _logger.LogError(ex, "Failed to update role with RoleId {RoleId}", id);
                        return new Result<VariationOptionDTO>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }
    }
}
