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
        private readonly ILogger<IRoleRepository> _logger;
        public RoleRepository(IOptions<DatabaseSettings> options, ILogger<IRoleRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
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
                                return new Result<RoleDTO>(true, "role_added_successfully", insertedRole);
                            }
                            return new Result<RoleDTO>(false, "failed_to_add_role", null, 500);
                        }
                    } 
                    catch (Exception ex) 
                    {
                        _logger.LogError(ex, "Failed to add a new role with NameEn {NameEn} and NameAr {NameAr}", newRole.NameEn, newRole.NameAr);
                        return new Result<RoleDTO>(false, "internal_server_error", null, 500);
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
                            return new Result<bool>(true, "role_deleted_successfully", true);
                        }
                        return new Result<bool>(false, "role_not_found", false, 404);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to delete role with RoleId {RoleId}", id);
                        return new Result<bool>(false, "internal_server_error", false, 500);
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
                                return new Result<IEnumerable<RoleDTO>>(true, "roles_retrieved_successfully", roles);
                            }
                            return new Result<IEnumerable<RoleDTO>>(false, "roles_not_found", null, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve all roles from database");
                        return new Result<IEnumerable<RoleDTO>>(false, "internal_server_error", null, 500);
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
                                return new Result<RoleDTO>(true, "role_retrieved_successfully", role);
                            }
                            return new Result<RoleDTO>(false, "role_not_found", null, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve role with RoleId {RoleId}", id);
                        return new Result<RoleDTO>(false, "internal_server_error", null, 500);
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
                            return new Result<bool>(true, "role_updated_successfully", true);
                        }
                        return new Result<bool>(false, "role_not_found", false, 404);
                    }
                    catch (SqlException ex)
                    {
                        _logger.LogError(ex, "Failed to update role with RoleId {RoleId}", id);
                        return new Result<bool>(false, "internal_server_error", false, 500);
                    }
                }
            }
        }
    }
}
