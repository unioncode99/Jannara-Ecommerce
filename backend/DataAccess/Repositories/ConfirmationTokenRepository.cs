using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Token;
using Jannara_Ecommerce.Enums;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class ConfirmationTokenRepository : IConfirmationTokenRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ICustomerRepository> _logger;

        public ConfirmationTokenRepository(IOptions<DatabaseSettings> options, ILogger<ICustomerRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }

        public async Task<Result<int>> AddNewAsync(ConfirmationTokenDTO passwordResetTokenDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO ConfirmationTokens
(
    user_id,
    token,
    purpose,
    verification_code,
    expires_at,
    is_used
)
VALUES
(
    @user_id,
    @token,
    @purpose,
    @verification_code,
    @expires_at,
    @is_used
);
SELECT SCOPE_IDENTITY();
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@user_id", passwordResetTokenDTO.UserId);
                    command.Parameters.AddWithValue("@verification_code", passwordResetTokenDTO.Code);
                    command.Parameters.AddWithValue("@token", passwordResetTokenDTO.Token);
                    command.Parameters.AddWithValue("@purpose",(int) passwordResetTokenDTO.Purpose);
                    command.Parameters.AddWithValue("@expires_at", passwordResetTokenDTO.ExpireAt);
                    command.Parameters.AddWithValue("@is_used", passwordResetTokenDTO.IsUsed);


                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int id = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (id > 0)
                        {
                            return new Result<int>(true, "token_added_successfully", id);
                        }
                        else
                        {
                            return new Result<int>(false, "failed_to_add_token", -1);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to add token");
                        return new Result<int>(false, "internal_server_error", -1, 500);
                    }

                }
            }
        }


        public async Task<Result<ConfirmationTokenDTO>> GetByTokenAsync(string token)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM ConfirmationTokens WHERE token = @token";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@token", token);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                ConfirmationTokenDTO resetTokenDTO = new ConfirmationTokenDTO
                                (
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetInt32(reader.GetOrdinal("user_id")),
                                 reader.GetString(reader.GetOrdinal("token")),
                                 reader.GetString(reader.GetOrdinal("verification_code")),
                                 (ConfirmationPurpose)reader.GetByte(reader.GetOrdinal("purpose")),
                                 reader.GetDateTime(reader.GetOrdinal("expires_at")),
                                 reader.GetBoolean(reader.GetOrdinal("is_used"))
                                 );
                                return new Result<ConfirmationTokenDTO>(true, "Token found successfully", resetTokenDTO);
                            }
                            else
                            {
                                return new Result<ConfirmationTokenDTO>(false, "Token not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<ConfirmationTokenDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public async Task<Result<ConfirmationTokenDTO>> GetByCodeAsync(string code)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM ConfirmationTokens WHERE verification_code = @code";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@code", code);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                ConfirmationTokenDTO resetTokenDTO = new ConfirmationTokenDTO
                                (
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetInt32(reader.GetOrdinal("user_id")),
                                 reader.GetString(reader.GetOrdinal("token")),
                                 reader.GetString(reader.GetOrdinal("verification_code")),
                                 (ConfirmationPurpose)reader.GetByte(reader.GetOrdinal("purpose")),
                                 reader.GetDateTime(reader.GetOrdinal("expires_at")),
                                 reader.GetBoolean(reader.GetOrdinal("is_used"))
                                 );
                                return new Result<ConfirmationTokenDTO>(true, "Token found successfully", resetTokenDTO);
                            }
                            else
                            {
                                return new Result<ConfirmationTokenDTO>(false, "Token not found.", null, 404);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<ConfirmationTokenDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }

        public async Task<Result<bool>> MarkAsUsedAsync(int userId, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
UPDATE ConfirmationTokens
SET 
    is_used = 1
WHERE user_id = @user_id;
select @@ROWCOUNT";
            using (SqlCommand command = new SqlCommand(query, conn, tran))
            {
                command.Parameters.AddWithValue("@user_id", userId);
                object result = await command.ExecuteScalarAsync();
                int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                if (rowAffected > 0)
                {
                    return new Result<bool>(true, "Token marked as used successfully.", true);
                }
                else
                {
                    return new Result<bool>(false, "Failed to mark token as used.", false);
                }


            }
        }

        public async Task<Result<bool>> MarkAsUsedAsync(int userId)
        {
            string query = @"
UPDATE ConfirmationTokens
SET 
    is_used = 1
WHERE user_id = @userId;
select @@ROWCOUNT";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    try
                    {
                        await connection.OpenAsync();
                        object result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                        {
                            return new Result<bool>(true, "Token marked as used successfully.", true);
                        }
                        else
                        {
                            return new Result<bool>(false, "Failed to mark token as used.", false, 500);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to to mark as used  for UserId {UserId}", userId);
                        return new Result<bool>(false, "An unexpected error occurred on the server", false, 500);
                    }

    


                }
            }
            
        }

    }
}
