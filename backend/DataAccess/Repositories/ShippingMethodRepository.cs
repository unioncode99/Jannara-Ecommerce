using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Address;
using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.ShippingMethod;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;
using System.Text.Json;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class ShippingMethodRepository : IShippingMethodRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<IShippingMethodRepository> _logger;
        public ShippingMethodRepository(IOptions<DatabaseSettings> options, ILogger<IShippingMethodRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }


        public async Task<Result<IEnumerable<ShippingMethodDTO>>> GetAllActiveAsync()
        {
            //throw new NotImplementedException();

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"Select * from ShippingMethods";
                using (var command = new SqlCommand(query, connection))
                {
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {

                            var shippingMethods = new List<ShippingMethodDTO>();
                            while (await reader.ReadAsync())
                            {
                                shippingMethods.Add(new ShippingMethodDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetString(reader.GetOrdinal("code")),
                                    reader.GetString(reader.GetOrdinal("name_en")),
                                    reader.GetString(reader.GetOrdinal("name_ar")),
                                    reader.GetDecimal(reader.GetOrdinal("base_price")),
                                    reader.GetDecimal(reader.GetOrdinal("price_per_kg")),
                                    reader.GetDecimal(reader.GetOrdinal("free_over")),
                                    reader.GetByte(reader.GetOrdinal("days_min")),
                                    reader.GetByte(reader.GetOrdinal("days_max")),
                                    reader.GetBoolean(reader.GetOrdinal("is_active")),
                                    reader.GetByte(reader.GetOrdinal("sort_order")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                               ));
                            }
                            if (shippingMethods.Count() < 1)
                            {
                                return new Result<IEnumerable<ShippingMethodDTO>>(false, "shipping_methods_not_found", null, 404);
                            }

                            return new Result<IEnumerable<ShippingMethodDTO>>(true, "shipping_methods_retrieved_successfully", shippingMethods);


                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve all shipping_methods");
                        return new Result<IEnumerable<ShippingMethodDTO>>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }
    
    }
}
