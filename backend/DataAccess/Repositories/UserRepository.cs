using Azure.Core;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.Dtos.User;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Enums;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        public UserRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
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
                        return new Result<UserPublicDTO>(true, "User added successfully.", insertedUser);
                    }
                    return new Result<UserPublicDTO>(false, "Failed to add User.", null, 500);

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
                            return new Result<bool>(true, "User deleted successfully.", true);
                        return new Result<bool>(false, "User not found.", false, 404);
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }

        public async Task<Result<PagedResponseDTO<UserPublicDTO>>> GetAllAsync(int pageNumber = 1, int pageSize = 20, int? currentUserId = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
SELECT COUNT(*) AS total FROM Users;

SELECT
    U.id  ,
    U.person_id,
    U.email,
    U.username,
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
ORDER BY U.id
OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;

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

                            var users = new List<UserPublicDTO>();
                            while (await reader.ReadAsync())
                            {
                                var rolesJson = reader.IsDBNull(reader.GetOrdinal("roles_json"))
                                       ? "[]"
                                       : reader.GetString(reader.GetOrdinal("roles_json"));

                                var rolesList = JsonSerializer.Deserialize<List<UserRoleInfoDTO>>(rolesJson);
                                users.Add(new UserPublicDTO(
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetInt32(reader.GetOrdinal("person_id")),
                                    reader.GetString(reader.GetOrdinal("email")),
                                    reader.GetString(reader.GetOrdinal("username")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at")),
                                    rolesList ?? new List<UserRoleInfoDTO>()
                                ));
                            }
                            if (users.Count() < 1)
                                return new Result<PagedResponseDTO<UserPublicDTO>>(false, "No user found!", null, 404);

                            var response = new PagedResponseDTO<UserPublicDTO>(total, pageNumber, pageSize, users);

                            return new Result<PagedResponseDTO<UserPublicDTO>>(true, "Users retureved successfully", response);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<PagedResponseDTO<UserPublicDTO>>(false, "An unexpected error occurred on the server.", null, 500);
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
    Users.id          AS UserId,
	Users.person_id,
	Users.email,
    Users.username,
	Users.password,
    Users.created_at  AS user_created_at,
    Users.updated_at  AS user_updated_at,

    Roles.id          AS role_id,
    Roles.name_ar,
    Roles.name_en,
    UserRoles.is_active,
    Roles.created_at  AS role_created_at,
    Roles.updated_at  AS role_updated_at
FROM Roles
INNER JOIN UserRoles ON Roles.id = UserRoles.role_id
INNER JOIN Users ON UserRoles.user_id = Users.id
WHERE Users.email = @email;
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    UserDTO user = null;

                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                if (user == null)
                                {
                                    user = new UserDTO
                                            (
                                                reader.GetInt32(reader.GetOrdinal("UserId")),
                                                reader.GetInt32(reader.GetOrdinal("person_id")),
                                                reader.GetString(reader.GetOrdinal("email")),
                                                reader.GetString(reader.GetOrdinal("username")),
                                                reader.GetString(reader.GetOrdinal("password")),
                                                reader.GetDateTime(reader.GetOrdinal("user_created_at")),
                                                reader.GetDateTime(reader.GetOrdinal("user_updated_at")),
                                                  new List<UserRoleInfoDTO>()
                                            );
                                }
                                user.Roles.Add(new UserRoleInfoDTO(
                                    reader.GetInt32(reader.GetOrdinal("role_id")),
                                    reader.GetString(reader.GetOrdinal("name_ar")),
                                    reader.GetString(reader.GetOrdinal("name_en")),
                                    reader.GetBoolean(reader.GetOrdinal("is_active")),
                                    reader.GetDateTime(reader.GetOrdinal("role_created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("role_updated_at")))
                                    );
                            }
                            if (user == null)
                                return new Result<UserDTO>(false, "User not found", null, 404);
                            return new Result<UserDTO>(true, "User found successfully", user);
                        }


                    }
                    catch (Exception ex)
                    {
                        return new Result<UserDTO>(false, "An unexpected error occurred on the server.", null, 500);
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
    Users.id          AS UserId,
	Users.person_id,
	Users.email,
    Users.username,
	Users.password,
    Users.created_at  AS user_created_at,
    Users.updated_at  AS user_updated_at,

    Roles.id          AS role_id,
    Roles.name_ar,
    Roles.name_en,
    UserRoles.is_active,
    Roles.created_at  AS role_created_at,
    Roles.updated_at  AS role_updated_at
FROM Roles
INNER JOIN UserRoles ON Roles.id = UserRoles.role_id
INNER JOIN Users ON UserRoles.user_id = Users.id
WHERE Users.id = @id;
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    UserDTO user = null;

                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                if (user == null)
                                {
                                    user = new UserDTO
                                            (
                                                reader.GetInt32(reader.GetOrdinal("UserId")),
                                                reader.GetInt32(reader.GetOrdinal("person_id")),
                                                reader.GetString(reader.GetOrdinal("email")),
                                                reader.GetString(reader.GetOrdinal("username")),
                                                reader.GetString(reader.GetOrdinal("password")),
                                                reader.GetDateTime(reader.GetOrdinal("user_created_at")),
                                                reader.GetDateTime(reader.GetOrdinal("user_updated_at")),
                                                  new List<UserRoleInfoDTO>()
                                            );
                                }
                                user.Roles.Add(new UserRoleInfoDTO(
                                    reader.GetInt32(reader.GetOrdinal("role_id")),
                                    reader.GetString(reader.GetOrdinal("name_ar")),
                                    reader.GetString(reader.GetOrdinal("name_en")),
                                    reader.GetBoolean(reader.GetOrdinal("is_active")),
                                    reader.GetDateTime(reader.GetOrdinal("role_created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("role_updated_at")))
                                    );
                            }
                            if (user == null)
                                return new Result<UserDTO>(false, "User not found", null, 404);
                            return new Result<UserDTO>(true, "User found successfully", user);
                        }


                    }
                    catch (Exception ex)
                    {
                        return new Result<UserDTO>(false, "An unexpected error occurred on the server.", null, 500);
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
                        return new Result<bool>(true, "User existence check completed.", isFound);

                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
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
                        return new Result<bool>(true, "User existence check completed.", isFound);

                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> UpdateAsync(int id, UserUpdateDTO updatedUser)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"

UPDATE Users
   SET email = @email
      ,username = @username
      ,password  = @password
 WHERE Id = @id
select @@ROWCOUNT";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@email", updatedUser.Email);
                    command.Parameters.AddWithValue("@username", updatedUser.Username);
                    command.Parameters.AddWithValue("@password", updatedUser.Password);


                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                            return new Result<bool>(true, "user updated successfully.", true);
                        return new Result<bool>(false, "Failed to update user.", false);
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
