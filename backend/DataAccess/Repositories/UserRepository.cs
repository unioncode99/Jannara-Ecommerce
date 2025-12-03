using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.Dtos.User;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Enums;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

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
 VALUES(
@person_id,
@email,
@username,
@password
)
Select * from Users Where Id  = (SELECT SCOPE_IDENTITY());
";
            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@person_id", personId);
                command.Parameters.AddWithValue("@email", newUser.Email);
                command.Parameters.AddWithValue("@username", newUser.Username);
                command.Parameters.AddWithValue("@password", newUser.Password);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        UserPublicDTO insertedUser = new UserPublicDTO
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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Users WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
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

        public async Task<Result<IEnumerable<UserPublicDTO>>> GetAllAsync(int pageNumber = 1, int pageSize = 20, int? currentUserId = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
SELECT
    U.id              AS user_id,
    U.person_id,
    U.email,
    U.username,
    U.created_at      AS user_created_at,
    U.updated_at      AS user_updated_at,

    R.id              AS role_id,
    R.name_ar,
    R.name_en,
    UR.is_active,
    R.created_at      AS role_created_at,
    R.updated_at      AS role_updated_at
FROM Users U
LEFT JOIN UserRoles UR ON U.id = UR.user_id
LEFT JOIN Roles R ON UR.role_id = R.id
ORDER BY U.id
OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int offset = (pageNumber - 1) * pageSize;
                    command.Parameters.AddWithValue("@offset", offset);
                    command.Parameters.AddWithValue("@pageSize", pageSize);


                    Dictionary<int, UserPublicDTO> users = new Dictionary<int, UserPublicDTO>();
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            

                            while (await reader.ReadAsync())
                            {
                                int userId = reader.GetInt32(reader.GetOrdinal("user_id"));

                                if (!users.TryGetValue(userId, out var user))
                                {
                                    users[userId] = new UserPublicDTO
                                    (
                                        userId,
                                        reader.GetInt32(reader.GetOrdinal("person_id")),
                                        reader.GetString(reader.GetOrdinal("email")),
                                        reader.GetString(reader.GetOrdinal("username")),
                                        reader.GetDateTime(reader.GetOrdinal("user_created_at")),
                                        reader.GetDateTime(reader.GetOrdinal("user_updated_at")),
                                        new List<UserRoleInfoDTO>()
                                    );
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("role_id")))
                                {
                                    users[userId].Roles.Add(
                                        new UserRoleInfoDTO(
                                            reader.GetInt32(reader.GetOrdinal("role_id")),
                                            reader.GetString(reader.GetOrdinal("name_ar")),
                                            reader.GetString(reader.GetOrdinal("name_en")),
                                            reader.GetBoolean(reader.GetOrdinal("is_active")),
                                            reader.GetDateTime(reader.GetOrdinal("role_created_at")),
                                            reader.GetDateTime(reader.GetOrdinal("role_updated_at"))
                                        )
                                    );
                                }
                            }

                            if (users.Count() > 0)
                                return new Result<IEnumerable<UserPublicDTO>>(true, "Users retrieved successfully", users.Values);
                            else
                                return new Result<IEnumerable<UserPublicDTO>>(false, "No user found!", null, 404);

                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<IEnumerable<UserPublicDTO>>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public async Task<Result<UserDTO>> GetByEmailAsync(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
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
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    UserDTO user = null;

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
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
            using (SqlConnection connection = new SqlConnection(_connectionString))
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
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    UserDTO user = null;

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT id FROM Users WHERE email = @email";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    bool isFound;
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT id FROM Users WHERE username = @username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    bool isFound;
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"

UPDATE Users
   SET email = @email
      ,username = @username
      ,password  = @password
 WHERE Id = @id
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
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
