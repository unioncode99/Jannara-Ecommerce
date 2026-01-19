using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.ProductCategory;
using Jannara_Ecommerce.DTOs.ProductRating;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.DTOs.UserRole;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Stripe;
using System.Collections.Generic;
using System.Text.Json;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class ProductRatingRepository : IProductRatingRepository
    {

        private readonly string _connectionString;
        private ILogger<IProductRatingRepository> _logger;
        public ProductRatingRepository(IOptions<DatabaseSettings> options, ILogger<IProductRatingRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }


        public async Task<Result<ProductRatingDTO>> AddNewAsync(ProductRatingCreateDTO newRating)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
declare @CustomerId int;

set @CustomerId = (select id from Customers where user_id = @UserId);

INSERT INTO [dbo].[ProductRatings]
           ([customer_id]
           ,[product_id]
           ,[rating]
           ,[review_text])

OUTPUT inserted.*
     VALUES
           (@CustomerId
           ,@ProductId
           ,@Rating
           ,@ReviewText
);
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", newRating.UserId);
                    command.Parameters.AddWithValue("@ProductId", newRating.ProductId);
                    command.Parameters.AddWithValue("@Rating", newRating.Rating);
                    command.Parameters.AddWithValue("@ReviewText", newRating.ReviewText ?? (object)DBNull.Value);

                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var insertedUser = new ProductRatingDTO
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("customer_id")),
                                    ProductId = reader.GetInt32(reader.GetOrdinal("product_id")),
                                    Rating = reader.GetByte(reader.GetOrdinal("rating")),
                                    ReviewText = reader.IsDBNull(reader.GetOrdinal("review_text")) ? null : reader.GetString(reader.GetOrdinal("review_text")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at")),
                                };
                                //return new Result<UserPublicDTO>(true, "User added successfully.", insertedUser);
                                return new Result<ProductRatingDTO>(true, "user_added_successfully", insertedUser);
                            }
                            //return new Result<UserPublicDTO>(false, "Failed to add User.", null, 500);
                            return new Result<ProductRatingDTO>(false, "failed_to_add_user", null, 500);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve user with UserId");
                        return new Result<ProductRatingDTO>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }

        public async Task<Result<PagedResponseDTO<ProductRatingDetailsDTO>>> GetAllAsync(ProductRatingFilterDTO filter)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
-- Count
select count(id) as total from ProductRatings
where product_id = @ProductId;

-- paged data
SELECT 
    pr.id,
    pr.customer_id,
    p.first_name + ' ' + p.last_name AS CustomerName,
	p.image_url as ProfileImage,
    pr.product_id,
    pr.rating,
    pr.review_text,
    pr.created_at,
    pr.updated_at

FROM ProductRatings pr
LEFT JOIN Customers c ON c.id = pr.customer_id
LEFT JOIN Users u ON u.id = c.user_id
LEFT JOIN People p ON p.id = u.person_id
where pr.product_id = @ProductId
ORDER BY id DESC
OFFSET @offset ROWS
FETCH NEXT @pageSize ROWS ONLY;

";

                using (var command = new SqlCommand(query, connection))
                {
                    int offset = (filter.PageNumber - 1) * filter.PageSize;
                    command.Parameters.AddWithValue("@offset", offset);
                    command.Parameters.AddWithValue("@pageSize", filter.PageSize);
                    command.Parameters.AddWithValue("@ProductId", filter.ProductId);

                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (!await reader.ReadAsync())
                            {
                                return new Result<PagedResponseDTO<ProductRatingDetailsDTO>>(
                                    false, "users_not_found", null, 404);
                            }

                            int total = reader.GetInt32(0);
                            await reader.NextResultAsync();
                            var Ratings = new List<ProductRatingDetailsDTO>();

                            while (await reader.ReadAsync()) 
                            {
                                Ratings.Add(new ProductRatingDetailsDTO
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("customer_id")),
                                    CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                                    ProfileImage = reader.GetString(reader.GetOrdinal("ProfileImage")),
                                    ProductId = reader.GetInt32(reader.GetOrdinal("product_id")),
                                    Rating = reader.GetByte(reader.GetOrdinal("rating")),
                                    ReviewText = reader.IsDBNull(reader.GetOrdinal("review_text")) ? null : reader.GetString(reader.GetOrdinal("review_text")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at")),
                                });
                            }

                            var response = new PagedResponseDTO<ProductRatingDetailsDTO>(total, filter.PageNumber, filter.PageSize, Ratings);
                            return new Result<PagedResponseDTO<ProductRatingDetailsDTO>>(true, "products_retrieved_successfully", response, 200);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve users for page {PageNumber} with page size {PageSize}", filter.PageNumber, filter.PageSize);
                        return new Result<PagedResponseDTO<ProductRatingDetailsDTO>>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }

        public async Task<Result<ProductRatingDTO>> UpdateAsync(int id, ProductRatingUpdateDTO updatedRating)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"

UPDATE ProductRatings
SET 
    rating = @Rating,
    review_text = @ReviewText
 WHERE id = @id;

SELECT
*
FROM ProductRatings
where id = @id
"
;
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@Rating", updatedRating.Rating);
                    command.Parameters.AddWithValue("@ReviewText", updatedRating.ReviewText ?? (object)DBNull.Value);
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var insertedUser = new ProductRatingDTO
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("customer_id")),
                                    ProductId = reader.GetInt32(reader.GetOrdinal("product_id")),
                                    Rating = reader.GetByte(reader.GetOrdinal("rating")),
                                    ReviewText = reader.IsDBNull(reader.GetOrdinal("review_text")) ? null : reader.GetString(reader.GetOrdinal("review_text")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at")),
                                };

                                return new Result<ProductRatingDTO>(true, "product_category_updated_successfully", insertedUser);
                            }
                            return new Result<ProductRatingDTO>(false, "failed_to_update_product_category", null, 500);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to update product_category with product_category id UpdateAsync", ex);
                        return new Result<ProductRatingDTO>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
DELETE FROM ProductRatings
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
