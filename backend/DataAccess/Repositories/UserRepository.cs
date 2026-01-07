using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Order;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.DTOs.UserRole;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private ILogger<IUserRepository> _logger;
        public UserRepository(IOptions<DatabaseSettings> options, ILogger<IUserRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }
        public async Task<Result<UserPublicDTO>> AddNewAsync(int personId, UserCreateDTO newUser, SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"

INSERT INTO Users
           (
person_id,
email,
username,
password)
OUTPUT inserted.*
 VALUES(
@person_id,
@email,
@username,
@password
)
";
            using (var command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@person_id", personId);
                command.Parameters.AddWithValue("@email", newUser.Email);
                command.Parameters.AddWithValue("@username", newUser.Username);
                command.Parameters.AddWithValue("@password", newUser.Password);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var insertedUser = new UserPublicDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetInt32(reader.GetOrdinal("person_id")),
                            reader.GetString(reader.GetOrdinal("email")),
                            reader.GetString(reader.GetOrdinal("username")),
                            reader.GetDateTime(reader.GetOrdinal("created_at")),
                            reader.GetDateTime(reader.GetOrdinal("updated_at")),
                            new List<UserRoleInfoDTO>()
                       );
                        //return new Result<UserPublicDTO>(true, "User added successfully.", insertedUser);
                        return new Result<UserPublicDTO>(true, "user_added_successfully", insertedUser);
                    }
                    //return new Result<UserPublicDTO>(false, "Failed to add User.", null, 500);
                    return new Result<UserPublicDTO>(false, "failed_to_add_user", null, 500);

                }

            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Users WHERE Id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                            return new Result<bool>(true, "user_deleted_successfully", true);
                        return new Result<bool>(false, "user_not_found", false, 404);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to delete user with UserId {UserId}", id);
                        return new Result<bool>(false, "internal_server_error", false, 500);
                    }

                }
            }
        }

        public async Task<Result<PagedResponseDTO<UserDetailsDTO>>> GetAllAsync(int pageNumber = 1, int pageSize = 20, int? currentUserId = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
DECLARE @isSuperAdmin BIT = 0;
DECLARE @isAdmin BIT = 0;
DECLARE @json NVARCHAR(MAX);

select count(id) as total from users;

-- Detect current user's role
SELECT
    @isSuperAdmin = MAX(CASE WHEN R.name_en = 'SuperAdmin' THEN 1 ELSE 0 END),
    @isAdmin = MAX(CASE WHEN R.name_en = 'Admin' THEN 1 ELSE 0 END)
FROM UserRoles UR
JOIN Roles R ON UR.role_id = R.id
WHERE UR.user_id = @currentUserId
  AND UR.is_active = 1;
-- Users
SELECT @json = (SELECT
    U.id AS Id,
    U.person_id AS PersonId,
    U.email AS Email,
    U.username AS Username,
    U.created_at AS CreatedAt,
    U.updated_at AS UpdatedAt,
    (
	-- Roles
        SELECT
            UR.id AS Id,
            R.name_ar AS NameAr,
            R.name_en AS NameEn,
            UR.is_active AS IsActive,
            UR.created_at AS CreatedAt,
            UR.updated_at AS UpdatedAt
        FROM UserRoles UR
        JOIN Roles R ON UR.role_id = R.id
        WHERE UR.user_id = U.id
        FOR JSON PATH
    ) AS Roles,
	-- Person Info
	JSON_QUERY(
	(
	Select 
	p.id AS Id,
	p.first_name As FirstName,
	p.last_name As LastName,
	p.phone As Phone,
	p.image_url As ImageUrl,
	p.gender As Gender,
	CASE P.gender
        WHEN 0 THEN 'Unknown'
        WHEN 1 THEN 'Male'
        WHEN 2 THEN 'Female'
        WHEN 3 THEN 'Other'
    END AS GenderNameEn,
    CASE P.gender
        WHEN 0 THEN N'غير محدد'
        WHEN 1 THEN N'ذكر'
        WHEN 2 THEN N'أنثى'
        WHEN 3 THEN N'آخر'
    END AS GenderNameAr,
	p.date_of_birth As DateOfBirth,
	p.created_at As CreatedAt,
	p.updated_at As UpdatedAt
	from People p where id = U.person_id
	for JSON PAth, WITHOUT_ARRAY_WRAPPER
	)) AS Person
FROM Users U
WHERE
(
    -- SuperAdmin → all users (exclude himself)
    @isSuperAdmin = 1 And (U.id <>  @currentUserId)

    OR

    -- Admin → Seller & Customer only (exclude himself)
    (
        @isAdmin = 1
        AND U.id <> @currentUserId
        AND EXISTS (
            SELECT 1
            FROM UserRoles UR
            JOIN Roles R ON UR.role_id = R.id
            WHERE UR.user_id = U.id
              AND UR.is_active = 1
              AND R.name_en IN ('Seller', 'Customer')
        )
    )
)
-- Seller / Customer → condition never matches → returns 0 rows
ORDER BY U.id
OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY
FOR JSON PATH
);
SELEct @json as FUllJSON;

";

                using (var command = new SqlCommand(query, connection))
                {
                    int offset = (pageNumber - 1) * pageSize;
                    command.Parameters.AddWithValue("@offset", offset);
                    command.Parameters.AddWithValue("@pageSize", pageSize);
                    command.Parameters.AddWithValue("@currentUserId", currentUserId);


                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (!await reader.ReadAsync())
                            {
                                return new Result<PagedResponseDTO<UserDetailsDTO>>(
                                    false, "users_not_found", null, 404);
                            }

                            int total = reader.GetInt32(0);
                            await reader.NextResultAsync();
                            await reader.ReadAsync();
                          
                            // Read the entire JSON as a string first
                            string json = reader.IsDBNull(0)
                            ? "[]"
                            : reader.GetString(0);

                            //var users = JsonSerializer.Deserialize<IEnumerable<UserDetailsDTO>>(
                            //    json,
                            //    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                            //);
                            var users = JsonSerializer.Deserialize<UserDetailsDTO[]>(
                            json,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                        );
                            var response = new PagedResponseDTO<UserDetailsDTO>(total, pageNumber, pageSize, users);
                            return new Result<PagedResponseDTO<UserDetailsDTO>>(true, "products_retrieved_successfully", response, 200);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve users for page {PageNumber} with page size {PageSize}", pageNumber, pageSize);
                        return new Result<PagedResponseDTO<UserDetailsDTO>>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }

        public async Task<Result<UserDTO>> GetByEmailAsync(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
SELECT
    U.id  ,
    U.person_id,
    U.email,
    U.username,
    U.is_confirmed,
    U.password,
    U.created_at ,
    U.updated_at ,
    (
        SELECT 
            UR.id as Id,
            R.name_ar as NameAr,
            R.name_en as NameEn,
            UR.is_active as IsActive,
            UR.created_at as CreatedAt,
            UR.updated_at as UpdateAt
        FROM UserRoles UR
        JOIN Roles R ON UR.role_id = R.id
        WHERE UR.user_id = U.id
        FOR JSON PATH
    ) AS roles_json
FROM Users U
where email = @email
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);

                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var rolesJson = reader.IsDBNull(reader.GetOrdinal("roles_json"))
                                       ? "[]"
                                       : reader.GetString(reader.GetOrdinal("roles_json"));

                                var rolesList = JsonSerializer.Deserialize<List<UserRoleInfoDTO>>(rolesJson);
                                var user = new UserDTO(
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetInt32(reader.GetOrdinal("person_id")),
                                    reader.GetString(reader.GetOrdinal("email")),
                                    reader.GetString(reader.GetOrdinal("username")),
                                    reader.GetBoolean(reader.GetOrdinal("is_confirmed")),
                                    reader.GetString(reader.GetOrdinal("password")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at")),
                                    rolesList ?? new List<UserRoleInfoDTO>()
                                );
                                return new Result<UserDTO>(true, "user_retrieved_successfully", user);
                            }
                            return new Result<UserDTO>(false, "user_not_found", null, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve user with Email {Email}", email);
                        return new Result<UserDTO>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }

        public async Task<Result<UserDTO>> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
SELECT
    U.id  ,
    U.person_id,
    U.email,
    U.username,
    U.is_confirmed,
    U.password,
    U.created_at ,
    U.updated_at ,
    (
        SELECT 
            UR.id as Id,
            R.name_ar as NameAr,
            R.name_en as NameEn,
            UR.is_active as IsActive,
            UR.created_at as CreatedAt,
            UR.updated_at as UpdateAt
        FROM UserRoles UR
        JOIN Roles R ON UR.role_id = R.id
        WHERE UR.user_id = U.id
        FOR JSON PATH
    ) AS roles_json
FROM Users U
where id = @id
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
                                var rolesJson = reader.IsDBNull(reader.GetOrdinal("roles_json"))
                                       ? "[]"
                                       : reader.GetString(reader.GetOrdinal("roles_json"));

                                var rolesList = JsonSerializer.Deserialize<List<UserRoleInfoDTO>>(rolesJson);
                                var user =  new UserDTO(
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetInt32(reader.GetOrdinal("person_id")),
                                    reader.GetString(reader.GetOrdinal("email")),
                                    reader.GetString(reader.GetOrdinal("username")),
                                    reader.GetBoolean(reader.GetOrdinal("is_confirmed")),
                                    reader.GetString(reader.GetOrdinal("password")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at")),
                                    rolesList ?? new List<UserRoleInfoDTO>()
                                );
                                return new Result<UserDTO>(true, "user_retrieved_successfully", user);
                            }                         
                            return new Result<UserDTO>(false, "user_not_found", null, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve user with UserId {UserId}", id);
                        return new Result<UserDTO>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> IsExistByEmail(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT id FROM Users WHERE email = @email";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    bool isFound;
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            isFound = reader.HasRows;
                        }
                        return new Result<bool>(true, "user_exists", isFound);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to check existence of user with Email {Email}", email);
                        return new Result<bool>(false, "internal_server_error", false, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> IsExistByUsername(string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT id FROM Users WHERE username = @username";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    bool isFound;
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            isFound = reader.HasRows;
                        }
                        return new Result<bool>(true, "user_exists", isFound);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to check existence of user with Username {Username}", username);
                        return new Result<bool>(false, "internal_server_error", false, 500);
                    }

                }
            }
        }

        public async Task<Result<UserDTO>> UpdateAsync(int id, UserUpdateDTO updatedUser)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"

UPDATE Users
   SET username = @username
 WHERE Id = @id;
SELECT
    U.id  ,
    U.person_id,
    U.email,
    U.username,
    U.is_confirmed,
    U.password,
    U.created_at ,
    U.updated_at ,
    (
        SELECT 
            UR.id as Id,
            R.name_ar as NameAr,
            R.name_en as NameEn,
            UR.is_active as IsActive,
            UR.created_at as CreatedAt,
            UR.updated_at as UpdateAt
        FROM UserRoles UR
        JOIN Roles R ON UR.role_id = R.id
        WHERE UR.user_id = U.id
        FOR JSON PATH
    ) AS roles_json
FROM Users U
where id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@username", updatedUser.Username);


                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var rolesJson = reader.IsDBNull(reader.GetOrdinal("roles_json"))
                                       ? "[]"
                                       : reader.GetString(reader.GetOrdinal("roles_json"));

                                var rolesList = JsonSerializer.Deserialize<List<UserRoleInfoDTO>>(rolesJson);
                                var user = new UserDTO(
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetInt32(reader.GetOrdinal("person_id")),
                                    reader.GetString(reader.GetOrdinal("email")),
                                    reader.GetString(reader.GetOrdinal("username")),
                                    reader.GetBoolean(reader.GetOrdinal("is_confirmed")),
                                    reader.GetString(reader.GetOrdinal("password")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at")),
                                    rolesList ?? new List<UserRoleInfoDTO>()
                                );
                                return new Result<UserDTO>(true, "user_updated_successfully", user);
                            }
                            return new Result<UserDTO>(false, "failed_to_update_user", null, 500);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to update user with UserId {UserId}", id);
                        return new Result<UserDTO>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> ResetPasswordAsync(int id, string newPassword, SqlConnection conn, SqlTransaction transaction)
        {
            string query = @"
UPDATE Users
SET 
    password = @newPassword
WHERE id = @id;
select @@ROWCOUNT";
            using (SqlCommand command = new SqlCommand(query, conn, transaction))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@newPassword", newPassword);
                object result = await command.ExecuteScalarAsync();
                int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                if (rowAffected > 0)
                {
                    return new Result<bool>(true, "Password changed successfully.", true);
                }
                else
                {
                    return new Result<bool>(false, "Failed to  change password.", false);
                }



            }
        }
        public async Task<Result<bool>> ResetPasswordAsync(ChangePasswordDTO resetPasswordDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                string query = @"
BEGIN TRY

    --  Validate user exists
    IF NOT EXISTS (SELECT 1 FROM Users WHERE id = @userId)
        THROW 50001, 'User does not exist.', 1;

    -- Update password
    UPDATE Users
    SET [password] = @newPassword
    WHERE id = @userId;

END TRY
BEGIN CATCH
    THROW;
END CATCH
";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@oldPassword", resetPasswordDTO.OldPassword);
                    command.Parameters.AddWithValue("@newPassword", resetPasswordDTO.NewPassword);
                    command.Parameters.AddWithValue("@userId", resetPasswordDTO.UserId);
                    try
                    {
                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected == 0)
                        {
                            return new Result<bool>(false, "can not reset password", false, 400);
                        }

                        return new Result<bool>(true, "password_changed_successfully", true, 200);
                    }
                    catch (SqlException sqlEx)
                    {
                        string message = sqlEx.Number switch
                        {
                            50001 => "User does not exist",
                            50002 => "Old password is incorrect",
                            _ => "internal_server_error"
                        };
                        _logger.LogError(sqlEx, "SQL exception in ResetPasswordAsync");
                        return new Result<bool>(false, message, false, 400);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unexpected error in ResetPasswordAsync");
                        return new Result<bool>(false, "internal_server_error", false, 500);
                    }
                }
            }
        }

        public async Task<Result<bool>> MarkEmailAsConfirmed(int id)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"

UPDATE Users
   SET is_confirmed = 1
 WHERE Id = @id
select @@ROWCOUNT";
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
                            return new Result<bool>(true, "user_confirmed_successfully", true);
                        }
                        return new Result<bool>(false, "failed_to_confrim_user", false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to confirm user {ID}", id);
                        return new Result<bool>(false, "internal_server_error", false, 500);
                    }

                }
            }
        }
        
        

    }
}
