using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.PaymentMethod;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<IPaymentMethodRepository> _logger;
        public PaymentMethodRepository(IOptions<DatabaseSettings> options, ILogger<IPaymentMethodRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<PaymentMethodDTO>>> GetAllActiveAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"select * from PaymentMethods where is_active = 1";

                using (var command = new SqlCommand(query, connection))
                {
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var paymentMethods = new List<PaymentMethodDTO>();
                            while (await reader.ReadAsync())
                            {
                                paymentMethods.Add(new PaymentMethodDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("name_en")),
                                    reader.GetString(reader.GetOrdinal("name_ar")),
                                    reader.GetString(reader.GetOrdinal("description_en")),
                                    reader.GetString(reader.GetOrdinal("description_ar")),
                                    reader.GetBoolean(reader.GetOrdinal("is_active")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                               ));
                            }
                            if (paymentMethods.Count() < 1)
                            {
                                return new Result<IEnumerable<PaymentMethodDTO>>(false, "payment_methods_not_found", null, 404);
                            }
                            return new Result<IEnumerable<PaymentMethodDTO>>(true, "payment_methods_retrieved_successfully", paymentMethods);


                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve payment_methods");
                        return new Result<IEnumerable<PaymentMethodDTO>>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }
    }
}
