using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.DTOs.ProductCategory;
using Jannara_Ecommerce.DTOs.User;
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
                                     reader.IsDBNull(reader.GetOrdinal("parent_category_id")) ? null : reader.GetInt32(reader.GetOrdinal("parent_category_id")),
                                     reader.GetString(reader.GetOrdinal("name_en")),
                                     reader.GetString(reader.GetOrdinal("name_ar")),
                                     reader.IsDBNull(reader.GetOrdinal("description_en")) ? null : reader.GetString(reader.GetOrdinal("description_en")),
                                     reader.IsDBNull(reader.GetOrdinal("description_ar")) ? null : reader.GetString(reader.GetOrdinal("description_ar")),
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

        public async Task<Result<ProductCategoryDTO>> AddNewAsync(ProductCategoryCreateDTO newProductCategory)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO [dbo].[ProductCategories]
           ([parent_category_id]
           ,[name_en]
           ,[name_ar]
           ,[description_en]
           ,[description_ar])
OUTPUT inserted.*
VALUES
(
    @parent_category_id,
    @name_en,
    @name_ar,
    @description_en,
    @description_ar
)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@parent_category_id", newProductCategory.ParentCategoryId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@name_en", newProductCategory.NameEn);
                    command.Parameters.AddWithValue("@name_ar", newProductCategory.NameAr);
                    command.Parameters.AddWithValue("@description_en", newProductCategory.DescriptionEn ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@description_ar", newProductCategory.DescriptionAr ?? (object)DBNull.Value);
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var productCategory = new ProductCategoryDTO
                                 (
                                     reader.GetInt32(reader.GetOrdinal("id")),
                                     reader.IsDBNull(reader.GetOrdinal("parent_category_id")) ? null : reader.GetInt32(reader.GetOrdinal("parent_category_id")),
                                     reader.GetString(reader.GetOrdinal("name_en")),
                                     reader.GetString(reader.GetOrdinal("name_ar")),
                                     reader.IsDBNull(reader.GetOrdinal("description_en")) ? null : reader.GetString(reader.GetOrdinal("description_en")),
                                     reader.IsDBNull(reader.GetOrdinal("description_ar")) ? null : reader.GetString(reader.GetOrdinal("description_ar")),
                                     reader.GetDateTime(reader.GetOrdinal("created_at")),
                                     reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                 );
                                return new Result<ProductCategoryDTO>(true, "product_category_added_successfully", productCategory);
                            }
                            return new Result<ProductCategoryDTO>(false, "failed_to_add_product_category", null, 500);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to update product_category with product_category id AddNewAsync", ex);
                        return new Result<ProductCategoryDTO>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }

        public async Task<Result<ProductCategoryDTO>> UpdateAsync(int id, ProductCategoryUpdateDTO updateProductCategory)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"

UPDATE ProductCategories
   SET parent_category_id = @parent_category_id,
   name_en = @name_en,
   name_ar = @name_ar,
   description_en = @description_en,
   description_ar = @description_ar
 WHERE Id = @id;
SELECT
*
FROM ProductCategories
where id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@parent_category_id", updateProductCategory.ParentCategoryId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@name_en", updateProductCategory.NameEn);
                    command.Parameters.AddWithValue("@name_ar", updateProductCategory.NameAr);
                    command.Parameters.AddWithValue("@description_en", updateProductCategory.DescriptionEn ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@description_ar", updateProductCategory.DescriptionAr ?? (object)DBNull.Value);
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                              var  productCategory  = new ProductCategoryDTO
                               (
                                     reader.GetInt32(reader.GetOrdinal("id")),
                                     reader.IsDBNull(reader.GetOrdinal("parent_category_id")) ? null : reader.GetInt32(reader.GetOrdinal("parent_category_id")),
                                     reader.GetString(reader.GetOrdinal("name_en")),
                                     reader.GetString(reader.GetOrdinal("name_ar")),
                                     reader.IsDBNull(reader.GetOrdinal("description_en")) ? null : reader.GetString(reader.GetOrdinal("description_en")),
                                     reader.IsDBNull(reader.GetOrdinal("description_ar")) ? null : reader.GetString(reader.GetOrdinal("description_ar")),
                                     reader.GetDateTime(reader.GetOrdinal("created_at")),
                                     reader.GetDateTime(reader.GetOrdinal("updated_at"))
                               );
                                return new Result<ProductCategoryDTO>(true, "product_category_updated_successfully", productCategory);
                            }
                            return new Result<ProductCategoryDTO>(false, "failed_to_update_product_category", null, 500);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to update product_category with product_category id UpdateAsync", ex);
                        return new Result<ProductCategoryDTO>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
DELETE FROM ProductCategories
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
