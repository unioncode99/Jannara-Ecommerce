using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.Dtos;
using Jannara_Ecommerce.DTOs;
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
        public async Task<Result<UserPublicDTO>> AddNewAsync(UserDTO newUser)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"

INSERT INTO Users
           (
person_id,
email,
user_name,
password)
 VALUES(
@person_id,
@email,
@user_name,
@password
)
Select * from Users Where Id  = (SELECT SCOPE_IDENTITY());
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@person_id", newUser.PersonId);
                    command.Parameters.AddWithValue("@email", newUser.Email);
                    command.Parameters.AddWithValue("@user_name", newUser.Username);
                    command.Parameters.AddWithValue("@password", newUser.Password);


                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                UserPublicDTO insertedUser = new UserPublicDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetInt32(reader.GetOrdinal("person_id")),
                                    reader.GetString(reader.GetOrdinal("email")),
                                    reader.GetString(reader.GetOrdinal("user_name")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                               );
                                return new Result<UserPublicDTO>(true, "User added successfully.", insertedUser);
                            }
                            return new Result<UserPublicDTO>(false, "Failed to add User.", null, 500);

                        }


                    }
                    catch (Exception ex)
                    {
                        return new Result<UserPublicDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

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

        public async Task<Result<IEnumerable<UserPublicDTO>>> GetAllAsync(int? currentUserId, int pageNumber = 1, int pageSize = 20)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"select * from Users
OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY ;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int offset = (pageNumber - 1) * pageSize;
                    command.Parameters.AddWithValue("@offset", offset);
                    command.Parameters.AddWithValue("@pageSize", pageSize);


                    List<UserPublicDTO> users = new List<UserPublicDTO>();
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {

                            while (await reader.ReadAsync())
                            {
                                users.Add(new UserPublicDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetInt32(reader.GetOrdinal("person_id")),
                                    reader.GetString(reader.GetOrdinal("email")),
                                    reader.GetString(reader.GetOrdinal("user_name")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                               ));
                            }

                            if (users.Count() > 0)
                                return new Result<IEnumerable<UserPublicDTO>>(true, "Users retrieved successfully", users);
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
Select * from Users where email = @email;
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);


                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                UserDTO user = new UserDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetInt32(reader.GetOrdinal("person_id")),
                                    reader.GetString(reader.GetOrdinal("email")),
                                    reader.GetString(reader.GetOrdinal("user_name")),
                                    reader.GetString(reader.GetOrdinal("password")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                               );
                                return new Result<UserDTO>(true, "User retrieved successfully.", user);
                            }
                            return new Result<UserDTO>(false, "User not found.", null, 404);

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
Select * from Users where id = @id;
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
                                UserDTO user = new UserDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetInt32(reader.GetOrdinal("person_id")),
                                    reader.GetString(reader.GetOrdinal("email")),
                                    reader.GetString(reader.GetOrdinal("user_name")),
                                    reader.GetString(reader.GetOrdinal("password")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                               );
                                return new Result<UserDTO>(true, "User retrieved successfully.", user);
                            }
                            return new Result<UserDTO>(false, "User not found.", null, 404);

                        }


                    }
                    catch (Exception ex)
                    {
                        return new Result<UserDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> UpdateAsync(int id, UserDTO updatedUser)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"

UPDATE Users
   SET person_id = @person_id
      ,email = @email
      ,username = @username
      ,password  = @password
 WHERE Id = @id
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@person_id", updatedUser.PersonId);
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
