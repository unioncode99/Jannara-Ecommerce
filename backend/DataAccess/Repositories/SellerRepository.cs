using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Role;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class SellerRepository : ISellerRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ISellerRepository> _logger;
        public SellerRepository(IOptions<DatabaseSettings> options, ILogger<ISellerRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }
        public async Task<Result<SellerDTO>> AddNewAsync(int userId, SellerCreateDTO newSeller, SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"
INSERT INTO Sellers
           (user_id
           ,business_name
           ,website_url)
OUTPUT inserted.*
     VALUES
           (@user_id
           ,@business_name
           ,@website_url);
";
            using (var command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@user_id", userId);
                command.Parameters.AddWithValue("@business_name", newSeller.BusinessName);
                command.Parameters.AddWithValue("@website_url", newSeller.WebsiteUrl ?? (object)DBNull.Value);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var insertedSeller = new SellerDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetInt32(reader.GetOrdinal("user_id")),
                            reader.GetString(reader.GetOrdinal("business_name")),
                            reader.IsDBNull(reader.GetOrdinal("website_url")) ? null : reader.GetString(reader.GetOrdinal("website_url")),
                            reader.GetDateTime(reader.GetOrdinal("created_at")),
                            reader.GetDateTime(reader.GetOrdinal("updated_at"))
                       );
                        return new Result<SellerDTO>(true, "seller_added_successfully", insertedSeller);
                    }
                    return new Result<SellerDTO>(false, "failed_to_add_seller", null, 500);
                }
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Sellers WHERE Id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "seller_deleted_successfully", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "seller_not_found", false, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to delete seller with SellerId {SellerId}", id);
                        return new Result<bool>(false, "internal_server_error", false, 500);
                    }

                }
            }
        }

        public async Task<Result<PagedResponseDTO<SellerDTO>>> GetAllAsync(int pageNumber = 1, int pageSize = 20)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
select count(*) as total from Customers ;
select * from Sellers
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

                            var sellers = new List<SellerDTO>();
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
                            if (sellers.Count() < 1)
                            {
                                return new Result<PagedResponseDTO<SellerDTO>>(false, "sellers_not_found", null, 404);
                            }
                            var response = new PagedResponseDTO<SellerDTO>(total, pageNumber, pageSize, sellers);
                            return new Result<PagedResponseDTO<SellerDTO>>(true, "sellers_retrieved_successfully", response);


                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve sellers for page {PageNumber} with page size {PageSize}", pageNumber, pageSize);
                        return new Result<PagedResponseDTO<SellerDTO>>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }

        public async Task<Result<SellerDTO>> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
Select * from Sellers where id = @id;
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
                                var customer = new SellerDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetInt32(reader.GetOrdinal("user_id")),
                                    reader.GetString(reader.GetOrdinal("business_name")),
                                    reader.IsDBNull(reader.GetOrdinal("website_url")) ? null : reader.GetString(reader.GetOrdinal("website_url")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                               );
                                return new Result<SellerDTO>(true, "seller_retrieved_successfully", customer);
                            }
                            return new Result<SellerDTO>(false, "seller_not_found", null, 404);

                        }


                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve seller with SellerId {SellerId}", id);
                        return new Result<SellerDTO>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> UpdateAsync(int id, SellerUpdateDTO updatedSeller)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"

UPDATE Sellers
   SET business_name = @business_name
      ,website_url = @website_url
 WHERE Id = @id;
select @@ROWCOUNT";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@business_name", updatedSeller.BusinessName);
                    command.Parameters.AddWithValue("@website_url", updatedSeller.WebsiteUrl ?? (object) DBNull.Value);


                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                            return new Result<bool>(true, "seller_updated_successfully", true);
                        return new Result<bool>(false, "seller_not_found", false, 404);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to update seller with SellerId {SellerId}", id);
                        return new Result<bool>(false, "internal_server_error", false, 500);
                    }

                }
            }
        }
    
        public async Task<Result<RoleDTO>> BecomeACustomer(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
-- VARIABLES
DECLARE @RoleId INT;

BEGIN TRANSACTION;

BEGIN TRY

    -- Validate user exists
    IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @UserId)
        THROW 51001, 'User does not exist.', 1;

    -- Validate customer role exists
    SELECT 
        @RoleId = Id,
        @RoleNameEn = NameEn,
        @RoleNameAr = NameAr
    FROM Roles
    WHERE NameEn = 'customer';

    IF @RoleId IS NULL
        THROW 51002, 'Customer role not found.', 1;

    -- Validate not already customer
    IF EXISTS (SELECT 1 FROM Customers WHERE UserId = @UserId)
        THROW 51003, 'User is already a customer.', 1;

    -- Validate role not already assigned
    IF EXISTS (
        SELECT 1 
        FROM UserRoles 
        WHERE UserId = @UserId AND RoleId = @RoleId
    )
        THROW 51004, 'Customer role already assigned.', 1;

    -- Insert into Customers
    INSERT INTO Customers (UserId)
    VALUES (@UserId);

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
                    command.Parameters.AddWithValue("@UserId", userId);
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
                                    "customer_role_added_successfully",
                                    role
                                );
                            }

                            return new Result<RoleDTO>(
                                false,
                                "failed_to_add_customer_role",
                                null,
                                500
                            );
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        string message = sqlEx.Number switch
                        {
                            51001 => "user_not_found",
                            51002 => "customer_role_not_found",
                            51003 => "already_customer",
                            51004 => "role_already_assigned",
                            _ => "internal_server_error"
                        };

                        _logger.LogError(sqlEx, "SQL error in BecomeCustomer");
                        return new Result<RoleDTO>(false, message, null, 400);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unexpected error in BecomeCustomer");
                        return new Result<RoleDTO>(false, "internal_server_error", null, 500);
                    }
                }
            }

        }
    }
}
