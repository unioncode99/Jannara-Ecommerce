using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Product;
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
    }
}
