using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ICustomerRepository> _logger;
        public CustomerRepository(IOptions<DatabaseSettings> options, ILogger<ICustomerRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }
        public async Task<Result<CustomerDTO>> AddNewAsync(int userId, SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"
INSERT INTO Customers
           (user_id)
OUTPUT inserted.*
     VALUES
           (@user_id);
";
            using (var command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@user_id", userId);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        CustomerDTO insertedCustomer = new CustomerDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetInt32(reader.GetOrdinal("user_id")),
                            reader.GetDateTime(reader.GetOrdinal("created_at")),
                            reader.GetDateTime(reader.GetOrdinal("updated_at"))
                       );
                        //return new Result<CustomerDTO>(true, "Customer added successfully.", insertedCustomer);
                        return new Result<CustomerDTO>(true, "customer_added_successfully", insertedCustomer);
                    }
                    //return new Result<CustomerDTO>(false, "Failed to add customer.", null, 500);
                    return new Result<CustomerDTO>(false, "failed_to_add_customer", null, 500);
                }
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Customers WHERE Id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                            return new Result<bool>(true, "customer_deleted_successfully", true);
                        return new Result<bool>(false, "customer_not_found", false, 404);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to delete customer with CustomerId {CustomerId}", id);
                        return new Result<bool>(false, "internal_server_error", false, 500);
                    }

                }
            }
        }

        public async Task<Result<PagedResponseDTO<CustomerDTO>>> GetAllAsync(int pageNumber = 1, int pageSize = 20)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
select count(*) as total from Customers 

select * from Customers
order by id
OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY ;
";
                using (var command = new SqlCommand(query, connection))
                {
                    int offset = (pageNumber - 1) * pageSize;
                    command.Parameters.AddWithValue("@offset", offset);
                    command.Parameters.AddWithValue("@pageSize", pageSize);


                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {

                            int total = 0;
                            if (await reader.ReadAsync())
                            {
                                total = reader.GetInt32(reader.GetOrdinal("total"));
                            }
                            await reader.NextResultAsync();

                            var customers = new List<CustomerDTO>();
                            while (await reader.ReadAsync())
                            {
                                customers.Add(new CustomerDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetInt32(reader.GetOrdinal("user_id")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                               ));
                            }
                            if (customers.Count() < 1)
                            {
                                return new Result<PagedResponseDTO<CustomerDTO>>(false, "customers_not_found", null, 404);
                            }
                                
                            PagedResponseDTO<CustomerDTO> response = new PagedResponseDTO<CustomerDTO>(total, pageNumber, pageSize, customers);
                            return new Result<PagedResponseDTO<CustomerDTO>>(true, "customers_retrieved_successfully", response);

                            
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve all customers for page {PageNumber} with page size {PageSize}", pageNumber, pageSize);
                        return new Result<PagedResponseDTO<CustomerDTO>>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }

        public async Task<Result<CustomerDTO>> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
Select * from Customers where id = @id;
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);


                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var customer = new CustomerDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetInt32(reader.GetOrdinal("user_id")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                               );
                                return new Result<CustomerDTO>(true, "customer_retrieved_successfully", customer);
                            }
                            return new Result<CustomerDTO>(false, "customer_not_found", null, 404);

                        }


                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve customer with CustomerId {CustomerId}", id);
                        return new Result<CustomerDTO>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> UpdateAsync(int id, CustomerDTO updatedCustomer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE Customers
   SET user_id = @user_id
 WHERE Id = @id
select @@ROWCOUNT";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@user_id", updatedCustomer.UserId);


                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                            return new Result<bool>(true, "customer_updated_successfully", true);
                        return new Result<bool>(false, "customer_not_found", false, 404);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to update customer with CustomerId {CustomerId}", id);
                        return new Result<bool>(false, "internal_server_error", false, 500);
                    }

                }
            }
        }
    }
}
