using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<IAddressRepository> _logger;

        public RefreshTokenRepository(IOptions<DatabaseSettings> options, ILogger<IAddressRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }

        public async Task<Result<RefreshTokenDTO>> AddNewAsync(int userId, string token, DateTime expires)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO RefreshTokens
(
user_id,
token,
expires
)
OUTPUT inserted.*
VALUES
(
@user_id,
@token,
@expires
);
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@user_id", userId);
                    command.Parameters.AddWithValue("@token", token);
                    command.Parameters.AddWithValue("@expires", expires);
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var insertedRefreshToken = new RefreshTokenDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetInt32(reader.GetOrdinal("user_id")),
                                    reader.GetString(reader.GetOrdinal("token")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("expires"))
                                );
                                return new Result<RefreshTokenDTO>(true, "Refresh token added successfully.", insertedRefreshToken);
                            }
                            _logger.LogError("Failed to add new refresh token for UserId {UserId}", userId);
                            return new Result<RefreshTokenDTO>(false, "Failed To Add refresh token", null, 500);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to add new refresh token for UserId {UserId}", userId);
                        return new Result<RefreshTokenDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }
                }
            }
        }


        public async Task<Result<RefreshTokenDTO>> GetInfoByTokenAsync(string token)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"Select * from RefreshToken Where token  = @token;";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@token", token);
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var refreshToken = new RefreshTokenDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetInt32(reader.GetOrdinal("user_id")),
                                    reader.GetString(reader.GetOrdinal("token")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("expires"))
                                );
                                return new Result<RefreshTokenDTO>(true, "Refresh token retrieved successfully.", refreshToken);
                            }
                            return new Result<RefreshTokenDTO>(false, "Refresh token Not Found", null, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve information for Token {Token}", token);
                        return new Result<RefreshTokenDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }
                }
            }
        }
    }
}
