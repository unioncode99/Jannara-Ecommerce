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

    }
}
