using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Token;
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
    verification_code,
    expires_at,
    is_used
)
VALUES
(
    @user_id,
    @token,
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
    
    }
}
