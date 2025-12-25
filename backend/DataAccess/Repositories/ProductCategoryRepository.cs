using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.IO.Pipelines;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<IAddressRepository> _logger;
        public ProductCategoryRepository(IOptions<DatabaseSettings> options, ILogger<IAddressRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }
        public async Task<Result<IEnumerable<ProductCategoryDTO>>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"select * from ProductCategories;";
                using (var command = new SqlCommand(query, connection))
                {
                    var productCategories = new List<ProductCategoryDTO>();
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                productCategories.Add(new ProductCategoryDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetString(reader.GetOrdinal("name_en")),
                                    reader.GetString(reader.GetOrdinal("name_ar")),
                                    reader.GetString(reader.GetOrdinal("description_en")),
                                    reader.GetString(reader.GetOrdinal("description_ar")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                ));
                            }
                            if (productCategories.Count > 0)
                            {
                                return new Result<IEnumerable<ProductCategoryDTO>>(true, "product_categories_retrieved_successfully", productCategories);
                            }
                            return new Result<IEnumerable<ProductCategoryDTO>>(false, "product_categories_not_found", null, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        return new Result<IEnumerable<ProductCategoryDTO>>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }

    }
}
