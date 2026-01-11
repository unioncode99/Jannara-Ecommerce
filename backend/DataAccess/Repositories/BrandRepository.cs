using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Brand;
using Jannara_Ecommerce.DTOs.Role;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<IBrandRepository> _logger;
        public BrandRepository(IOptions<DatabaseSettings> options, ILogger<IBrandRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<BrandDTO>>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"Select * from Brands order by id desc";
                using (var command = new SqlCommand(query, connection))
                {
                    var brands = new List<BrandDTO>();
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                brands.Add(new BrandDTO
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    NameEn = reader.GetString(reader.GetOrdinal("name_en")),
                                    NameAr = reader.GetString(reader.GetOrdinal("name_ar")),
                                    DescriptionEn = reader.GetString(reader.GetOrdinal("description_en")),
                                    DescriptionAr = reader.GetString(reader.GetOrdinal("description_ar")),
                                    LogoUrl = reader.GetString(reader.GetOrdinal("logo_url")),
                                    WebsiteUrl = reader.GetString(reader.GetOrdinal("website_url")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                });
                            }
                            if (brands.Count > 0)
                            {
                                return new Result<IEnumerable<BrandDTO>>(true, "brands_retrieved_successfully", brands);
                            }
                            return new Result<IEnumerable<BrandDTO>>(false, "brands_not_found", null, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve all brands from database");
                        return new Result<IEnumerable<BrandDTO>>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }


    }
}
