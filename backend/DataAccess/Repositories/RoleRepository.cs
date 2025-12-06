using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly string _connectionString;
        public RoleRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public async Task<Result<RoleDTO>> AddNewAsync(RoleDTO newRole)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO Roles
(
name_en,
name_ar
)
OUTPUT inserted.*
VALUES
(
@name_en,
@name_ar
);
";
                using (var command = new SqlCommand(query, connection)) 
                {
                    command.Parameters.AddWithValue("@name_en", newRole.NameEn);
                    command.Parameters.AddWithValue("@name_ar", newRole.NameAr);
                    try 
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync()) 
                        {
                            if (await reader.ReadAsync())
                            {
                                var insertedRole = new RoleDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetString(reader.GetOrdinal("name_en")),
                                    reader.GetString(reader.GetOrdinal("name_ar")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                );
                                return new Result<RoleDTO>(true, "Role added successfully.", insertedRole);
                            }
                            return new Result<RoleDTO>(false, "Failed To Add Role", null, 500);
                        }
                    } catch (Exception ex) 
                    {
                        return new Result<RoleDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }
                }
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Roles WHERE id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowsAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowsAffected > 0)
                        {
                            return new Result<bool>(true, "Role deleted successfully.", true);
                        }
                        return new Result<bool>(false, "Role Not Found", false, 404);
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }
                }
            }
        }

        public async Task<Result<IEnumerable<RoleDTO>>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"Select * from Roles";
                using (var command = new SqlCommand(query, connection))
                {
                    var roles = new List<RoleDTO>();
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                roles.Add(new RoleDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetString(reader.GetOrdinal("name_en")),
                                    reader.GetString(reader.GetOrdinal("name_ar")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                ));
                            }
                            if (roles.Count > 0)
                            {
                                return new Result<IEnumerable<RoleDTO>>(true, "Roles retrieved successfully", roles);
                            }
                            return new Result<IEnumerable<RoleDTO>>(false, "Roles Not Found", null, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<IEnumerable<RoleDTO>>(false, "An unexpected error occurred on the server.", null, 500);
                    }
                }
            }
        }

        public async Task<Result<RoleDTO>> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"Select * from Roles Where id  = @id;";
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
                                var role = new RoleDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetString(reader.GetOrdinal("name_en")),
                                    reader.GetString(reader.GetOrdinal("name_ar")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                );
                                return new Result<RoleDTO>(true, "Role retrieved successfully.", role);
                            }
                            return new Result<RoleDTO>(false, "Role Not Found", null, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<RoleDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }
                }
            }
        }

        public async Task<Result<bool>> UpdateAsync(int id, RoleDTO updatedRole)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE Roles
SET 
    name_en = @name_en,
    name_ar = @name_ar
WHERE Id = @id;
select @@ROWCOUNT
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@name_en", updatedRole.NameEn);
                    command.Parameters.AddWithValue("@name_ar", updatedRole.NameAr);
                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowsAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowsAffected > 0)
                        {
                            return new Result<bool>(true, "Role updated successfully.", true);
                        }
                        return new Result<bool>(false, "Role Not Found.", false, 404);
                    }
                    catch (SqlException ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }
                }
            }
        }
    }
}
