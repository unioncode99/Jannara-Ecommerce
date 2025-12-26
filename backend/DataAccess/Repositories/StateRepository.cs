using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Address;
using Jannara_Ecommerce.DTOs.State;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class StateRepository : IStateRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<IStateRepository> _logger;
        public StateRepository(IOptions<DatabaseSettings> options, ILogger<IStateRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<StateDTO>>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"Select * from States";
                using (var command = new SqlCommand(query, connection))
                {
                    var states = new List<StateDTO>();
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                states.Add(new StateDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetString(reader.GetOrdinal("code")),
                                    reader.GetString(reader.GetOrdinal("name_en")),
                                    reader.GetString(reader.GetOrdinal("name_ar")),
                                    reader.GetDecimal(reader.GetOrdinal("extra_fee_for_shipping")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                ));
                            }
                            if (states.Count > 0)
                            {
                                return new Result<IEnumerable<StateDTO>>(true, "states_retrieved_successfully", states);
                            }
                            return new Result<IEnumerable<StateDTO>>(false, "states_not_found", null, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to get all states");
                        return new Result<IEnumerable<StateDTO>>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }

    }
}
