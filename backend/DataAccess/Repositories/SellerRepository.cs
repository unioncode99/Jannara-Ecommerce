using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class SellerRepository : ISellerRepository
    {
        private readonly string _connectionString;
        public SellerRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public async Task<Result<SellerDTO>> AddNewAsync(SellerDTO newSeller, SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"
INSERT INTO Sellers
           (user_id
           ,business_name
           ,website_url)
     VALUES
           (@user_id
           ,@business_name
           ,@website_url);
Select * from Seller Where Id  = (SELECT SCOPE_IDENTITY());
";
            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@user_id", newSeller.UserId);
                command.Parameters.AddWithValue("@business_name", newSeller.BusinessName);
                command.Parameters.AddWithValue("@website_url", newSeller.WebsiteUrl ?? (object)DBNull.Value);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        SellerDTO insertedSeller = new SellerDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetInt32(reader.GetOrdinal("user_id")),
                            reader.GetString(reader.GetOrdinal("business_name")),
                            reader.IsDBNull(reader.GetOrdinal("website_url")) ? null : reader.GetString(reader.GetOrdinal("website_url")),
                            reader.GetDateTime(reader.GetOrdinal("created_at")),
                            reader.GetDateTime(reader.GetOrdinal("updated_at"))
                       );
                        return new Result<SellerDTO>(true, "Seller added successfully.", insertedSeller);
                    }
                    return new Result<SellerDTO>(false, "Failed to add seller.", null, 500);
                }
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Sellers WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "Seller deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Seller not found", false, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }

        public async Task<Result<IEnumerable<SellerDTO>>> GetAllAsync(int pageNumber = 1, int pageSize = 20)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"select * from Sellers
OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY ;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int offset = (pageNumber - 1) * pageSize;
                    command.Parameters.AddWithValue("@offset", offset);
                    command.Parameters.AddWithValue("@pageSize", pageSize);


                    List<SellerDTO> sellers = new List<SellerDTO>();
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {

                            while (await reader.ReadAsync())
                            {
                                sellers.Add(new SellerDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetInt32(reader.GetOrdinal("user_id")),
                                    reader.GetString(reader.GetOrdinal("business_name")),
                                    reader.IsDBNull(reader.GetOrdinal("website_url")) ? null : reader.GetString(reader.GetOrdinal("website_url")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                               ));
                            }

                            if (sellers.Count() > 0)
                                return new Result<IEnumerable<SellerDTO>>(true, "Sellers retrieved successfully", sellers);
                            else
                                return new Result<IEnumerable<SellerDTO>>(false, "No seller found!", null, 404);

                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<IEnumerable<SellerDTO>>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public async Task<Result<SellerDTO>> GetByIdAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
Select * from Sellers where id = @id;
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);


                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                SellerDTO customer = new SellerDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetInt32(reader.GetOrdinal("user_id")),
                                    reader.GetString(reader.GetOrdinal("business_name")),
                                    reader.IsDBNull(reader.GetOrdinal("website_url")) ? null : reader.GetString(reader.GetOrdinal("website_url")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                               );
                                return new Result<SellerDTO>(true, "Seller retrieved successfully.", customer);
                            }
                            return new Result<SellerDTO>(false, "Seller not found.", null, 404);

                        }


                    }
                    catch (Exception ex)
                    {
                        return new Result<SellerDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> UpdateAsync(int id, SellerDTO updatedSeller)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"

UPDATE Sellers
   SET user_id = user_id
      ,business_name = @business_name
      ,website_url = @website_url
 WHERE Id = @id;
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@user_id", updatedSeller.UserId);
                    command.Parameters.AddWithValue("@business_name", updatedSeller.BusinessName);
                    command.Parameters.AddWithValue("@website_url", updatedSeller.WebsiteUrl ?? (object) DBNull.Value);


                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                            return new Result<bool>(true, "Seller updated successfully.", true);
                        return new Result<bool>(false, "Seller not found.", false, 404);
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }
    }
}
