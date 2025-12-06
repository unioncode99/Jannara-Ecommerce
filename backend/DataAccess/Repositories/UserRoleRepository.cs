using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly string _connectionString;
        public UserRoleRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        

        public async Task<Result<UserRoleDTO>> AddNewAsync(int roleId, int userId, bool isActive, SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"
INSERT INTO UserRoles
(
user_id,
role_id,
is_active
)
OUTPUT inserted.*
VALUES
(
@user_id,
@role_id,
@is_active
);
";
            using (var command = new SqlCommand(query,connection,transaction))
            {
                command.Parameters.AddWithValue("@user_id", userId);
                command.Parameters.AddWithValue("@role_id", roleId);
                command.Parameters.AddWithValue("@is_active", isActive);
                try
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var insertedUserRole = new UserRoleDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetInt32(reader.GetOrdinal("role_id")),
                                reader.GetInt32(reader.GetOrdinal("user_id")),
                                reader.GetBoolean(reader.GetOrdinal("is_active")),
                                reader.GetDateTime(reader.GetOrdinal("created_at")),
                                reader.GetDateTime(reader.GetOrdinal("updated_at"))
                            );
                            return new Result<UserRoleDTO>(true, "User Role added successfully.", insertedUserRole);
                        }
                        return new Result<UserRoleDTO>(false, "Failed To Add Address", null, 500);
                    }
                }
                catch (Exception ex)
                {
                    return new Result<UserRoleDTO>(false, "An unexpected error occurred on the server.", null, 500);
                }
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM UserRoles WHERE id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("id", id);
                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowsAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowsAffected > 0)
                        {
                            return new Result<bool>(true, "User Role deleted successfully.", true);
                        }
                        return new Result<bool>(false, "User Role Not Found.", false, 404);
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }
                }
            }
        }
        public async Task<Result<IEnumerable<UserRoleDTO>>> GetAllAsync(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"Select * from UserRoles Where user_id  = @user_id;";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@user_id", userId);
                    var userRoles = new List<UserRoleDTO>();
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                userRoles.Add(new UserRoleDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetInt32(reader.GetOrdinal("role_id")),
                                    reader.GetInt32(reader.GetOrdinal("user_id")),
                                    reader.GetBoolean(reader.GetOrdinal("is_active")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                ));
                            }
                            if (userRoles.Count > 0)
                            {
                                return new Result<IEnumerable<UserRoleDTO>>(true, "User Roles retrieved successfully.", userRoles);
                            }
                            return new Result<IEnumerable<UserRoleDTO>>(false, "User Roles Not Found.", null, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<IEnumerable<UserRoleDTO>>(false, "An unexpected error occurred on the server.", null, 500);
                    }
                }
            }
        }

        public async Task<Result<UserRoleDTO>> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"Select * from UserRoles Where id  = @id;";
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
                                var userRole = new UserRoleDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetInt32(reader.GetOrdinal("role_id")),
                                    reader.GetInt32(reader.GetOrdinal("user_id")),
                                    reader.GetBoolean(reader.GetOrdinal("is_active")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                );
                                return new Result<UserRoleDTO>(true, "User Role retrieved successfully.", userRole);
                            }
                            return new Result<UserRoleDTO>(false, "User Role Not Found", null, 404);

                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<UserRoleDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }
                }
            }
        }

        public async Task<Result<bool>> UpdateAsync(int id, UserRoleDTO updatedUserRole)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE UserRoles
SET 
    role_id = @role_id,
    is_active = @is_active
WHERE id = @id;
select @@ROWCOUNT
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", updatedUserRole.Id);
                    command.Parameters.AddWithValue("@role_id", updatedUserRole.RoleId);
                    command.Parameters.AddWithValue("@is_active", updatedUserRole.IsActive);
                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowsAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowsAffected > 0)
                        {
                            return new Result<bool>(true, "User Role updated successfully.", true);
                        }
                        return new Result<bool>(false, "User Role Not Found.", false, 404);
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
