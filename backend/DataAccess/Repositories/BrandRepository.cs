using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Brand;
using Jannara_Ecommerce.DTOs.ProductCategory;
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

        public async Task<Result<BrandDTO>> AddNewAsync(BrandCreateDTO newBrand)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO [dbo].[Brands]
           ([name_en]
           ,[name_ar]
           ,[logo_url]
           ,[website_url]
           ,[description_en]
           ,[description_ar]
)
OUTPUT inserted.*
     VALUES
           @NameEn
           ,@NameAr
           ,@LogoUrl
           ,@WebsiteUrl
           ,@DescriptionEn
           ,@DescriptionAr
);
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NameEn", newBrand.NameEn);
                    command.Parameters.AddWithValue("@NameAr", newBrand.NameAr);
                    command.Parameters.AddWithValue("@LogoUrl", newBrand.LogoUrl);
                    command.Parameters.AddWithValue("@WebsiteUrl", newBrand.WebsiteUrl);
                    command.Parameters.AddWithValue("@DescriptionEn", newBrand.DescriptionEn ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DescriptionAr", newBrand.DescriptionAr ?? (object)DBNull.Value);
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var brand = new BrandDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("id")),
                                     reader.GetString(reader.GetOrdinal("name_en")),
                                     reader.GetString(reader.GetOrdinal("name_ar")),
                                     reader.GetString(reader.GetOrdinal("logo_url")),
                                     reader.GetString(reader.GetOrdinal("website_url")),
                                     reader.IsDBNull(reader.GetOrdinal("description_en")) ? null : reader.GetString(reader.GetOrdinal("description_en")),
                                     reader.IsDBNull(reader.GetOrdinal("description_ar")) ? null : reader.GetString(reader.GetOrdinal("description_ar")),
                                     reader.GetDateTime(reader.GetOrdinal("created_at")),
                                     reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                 );
                                return new Result<BrandDTO>(true, "product_category_added_successfully", brand);
                            }
                            return new Result<BrandDTO>(false, "failed_to_add_product_category", null, 500);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to update product_category with product_category id AddNewAsync", ex);
                        return new Result<BrandDTO>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }

        public async Task<Result<BrandDTO>> UpdateAsync(int id, BrandUpdateDTO updatedBrand)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                string query = @"

UPDATE Brands
   SET 
   name_en = @NameEn,
   name_ar = @NameAr,
   logo_url = @LogoUrl,
   website_url = @WebsiteUrl,
   description_en = @DescriptionEn,
   description_ar = @DescriptionAr
 WHERE Id = @id;
SELECT
*
FROM Brands
where id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@NameEn", updatedBrand.NameEn);
                    command.Parameters.AddWithValue("@NameAr", updatedBrand.NameAr);
                    command.Parameters.AddWithValue("@LogoUrl", updatedBrand.LogoUrl);
                    command.Parameters.AddWithValue("@WebsiteUrl", updatedBrand.WebsiteUrl);
                    command.Parameters.AddWithValue("@DescriptionEn", updatedBrand.DescriptionEn ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DescriptionAr", updatedBrand.DescriptionAr ?? (object)DBNull.Value);
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var brand = new BrandDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("id")),
                                     reader.GetString(reader.GetOrdinal("name_en")),
                                     reader.GetString(reader.GetOrdinal("name_ar")),
                                     reader.GetString(reader.GetOrdinal("logo_url")),
                                     reader.GetString(reader.GetOrdinal("website_url")),
                                     reader.IsDBNull(reader.GetOrdinal("description_en")) ? null : reader.GetString(reader.GetOrdinal("description_en")),
                                     reader.IsDBNull(reader.GetOrdinal("description_ar")) ? null : reader.GetString(reader.GetOrdinal("description_ar")),
                                     reader.GetDateTime(reader.GetOrdinal("created_at")),
                                     reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                 );
                                return new Result<BrandDTO>(true, "product_category_added_successfully", brand);
                            }
                            return new Result<BrandDTO>(false, "failed_to_add_product_category", null, 500);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to update product_category with product_category id UpdateAsync", ex);
                        return new Result<BrandDTO>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
DELETE FROM Brands
where id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    try
                    {
                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return new Result<bool>(true, "product_category_deleted_successfully", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "product_category_not_found", false, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to update product_category with product_category id DeleteAsync", ex);
                        return new Result<bool>(false, "internal_server_error", false, 500);
                    }

                }
            }
        }

    }
}
