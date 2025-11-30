using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.Dtos;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;
        public CustomerRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public async Task<Result<CustomerDTO>> AddNewAsync(CustomerDTO newCustomer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO Customers
           (user_id
           ,created_at
           ,updated_at)
     VALUES
           (@user_id
           ,@created_at
           ,@updated_at);
Select * from Cutomers Where Id  = (SELECT SCOPE_IDENTITY());
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@user_id", newCustomer.UserId);
                    command.Parameters.AddWithValue("@created_at", newCustomer.CreatedAt);
                    command.Parameters.AddWithValue("@updated_at", newCustomer.UpdatedAt);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                CustomerDTO insertedUser = new CustomerDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetInt32(reader.GetOrdinal("user_id")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                               );
                                return new Result<CustomerDTO>(true, "Customer added successfully.", insertedUser);
                            }
                            return new Result<CustomerDTO>(false, "Failed to add customer.", null);

                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<CustomerDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Cutomers WHERE Id = @id";
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
                            return new Result<bool>(true, "Customer deleted successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to delete customer.", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }

        public async Task<Result<IEnumerable<CustomerDTO>>> GetAllAsync(int pageNumber = 1, int pageSize = 20)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"select * from Customers
OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY ;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int offset = (pageNumber - 1) * pageSize;
                    command.Parameters.AddWithValue("@offset", offset);
                    command.Parameters.AddWithValue("@pageSize", pageSize);


                    List<CustomerDTO> customers = new List<CustomerDTO>();
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {

                            while (reader.Read())
                            {
                                customers.Add(new CustomerDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetInt32(reader.GetOrdinal("user_id")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                               ));
                            }

                            if (customers.Count() > 0)
                                return new Result<IEnumerable<CustomerDTO>>(true, "Doctors retrieved successfully", customers);
                            else
                                return new Result<IEnumerable<CustomerDTO>>(false, "No doctor found!", null, 404);

                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<IEnumerable<CustomerDTO>>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public async Task<Result<CustomerDTO>> GetByIdAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
Select * from Customers where id = @id;
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
                                CustomerDTO customer = new CustomerDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetInt32(reader.GetOrdinal("user_id")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                               );
                                return new Result<CustomerDTO>(true, "User retrieved successfully.", customer);
                            }
                            return new Result<CustomerDTO>(false, "Failed to retrieved User.", null);

                        }


                    }
                    catch (Exception ex)
                    {
                        return new Result<CustomerDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> UpdateAsync(int id, CustomerDTO updatedCustomer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE Customers
   SET user_id = @user_id
 WHERE Id = @id
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@user_id", updatedCustomer.UserId);


                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "Cutomer updated successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to update customer.", false);
                        }
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
