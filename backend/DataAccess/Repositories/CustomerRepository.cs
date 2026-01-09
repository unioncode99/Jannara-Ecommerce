using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Role;
using Jannara_Ecommerce.DTOs.Seller;
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

        public async Task<Result<RoleDTO>> BecomeASeller(BecomeSellerDTO becomeSellerDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
-- VARIABLES
DECLARE @RoleId INT;
DECLARE @RoleNameEn NVARCHAR(50);
DECLARE @RoleNameAr NVARCHAR(50);
DECLARE @SellerId INT;

BEGIN TRANSACTION;

BEGIN TRY

    -- Validate user exists
    IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @UserId)
        THROW 52001, 'User does not exist.', 1;

    -- Validate seller role exists
    SELECT 
        @RoleId = Id,
    FROM Roles
    WHERE name_en = 'seller';

    IF @RoleId IS NULL
        THROW 52002, 'Seller role not found.', 1;

    -- Validate not already seller
    IF EXISTS (SELECT 1 FROM Sellers WHERE UserId = @UserId)
        THROW 52003, 'User is already a seller.', 1;

    -- Validate role not already assigned
    IF EXISTS (
        SELECT 1 
        FROM UserRoles 
        WHERE UserId = @UserId AND RoleId = @RoleId
    )
        THROW 52004, 'Seller role already assigned.', 1;


    -- Insert into Sellers
    INSERT INTO Sellers (user_id, business_name, website_url)
    VALUES (@UserId, @BusinessName, @WebsiteUrl);

    SET @SellerId = SCOPE_IDENTITY();

    -- Assign role
    INSERT INTO UserRoles (UserId, RoleId)
    VALUES (@UserId, @RoleId);

    COMMIT TRANSACTION;

    -- Return role data
    SELECT 
        * from userRoles
 where user_id = @UserId AND role_id = @RoleId;

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
END CATCH
";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", becomeSellerDTO.UserId);
                    command.Parameters.AddWithValue("@BusinessName", becomeSellerDTO.BusinessName);
                    command.Parameters.AddWithValue("@WebsiteUrl",
                        string.IsNullOrWhiteSpace(becomeSellerDTO.WebsiteUrl)
                            ? (object)DBNull.Value
                            : becomeSellerDTO.WebsiteUrl
                    );

                    try
                    {
                        await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var role = new RoleDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetString(reader.GetOrdinal("name_en")),
                                    reader.GetString(reader.GetOrdinal("name_ar")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                );

                                return new Result<RoleDTO>(
                                    true,
                                    "seller_role_added_successfully",
                                    role
                                );
                            }

                            return new Result<RoleDTO>(
                                false,
                                "failed_to_add_seller_role",
                                null,
                                500
                            );
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        string message = sqlEx.Number switch
                        {
                            52001 => "user_not_found",
                            52002 => "seller_role_not_found",
                            52003 => "already_seller",
                            52004 => "role_already_assigned",
                            52005 => "business_name_required",
                            52006 => "invalid_website_url",
                            _ => "internal_server_error"
                        };

                        _logger.LogError(sqlEx, "SQL error in BecomeSeller");
                        return new Result<RoleDTO>(false, message, null, 400);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unexpected error in BecomeSeller");
                        return new Result<RoleDTO>(false, "internal_server_error", null, 500);
                    }
                }
            }


        }
    }
}
