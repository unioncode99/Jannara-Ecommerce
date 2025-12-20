using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.DTOs.CustomerWishlist;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class CustomerWishlistRepository : ICustomerWishlistRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<IAddressRepository> _logger;

        public CustomerWishlistRepository(IOptions<DatabaseSettings> options, ILogger<IAddressRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }

        public async Task<Result<CustomerWishlistDTO>> AddNewAsync(CustomerWishlistCreateDTO customerWishlist)
        {
            Console.WriteLine(customerWishlist.CustomerId);
            Console.WriteLine(customerWishlist.ProductId);
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"INSERT INTO CustomerWishlist
           (customer_id,
		   product_id)
OUTPUT inserted.*
     VALUES
           (@customerId,
            @productId);";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@customerId", customerWishlist.CustomerId);
                    command.Parameters.AddWithValue("@productId", customerWishlist.ProductId);
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                CustomerWishlistDTO insertedCustomerWishlist = new CustomerWishlistDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetInt32(reader.GetOrdinal("customer_id")),
                                    reader.GetInt32(reader.GetOrdinal("product_id")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                );
                                return new Result<CustomerWishlistDTO>(true, "customer_wishlist_added_successfully", insertedCustomerWishlist);
                            }
                            return new Result<CustomerWishlistDTO>(false, "failed_to_add_customer_wishlist", null, 500);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to add new customer Wishlist");
                        return new Result<CustomerWishlistDTO>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"DELETE FROM CustomerWishlist WHERE id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    try
                    {
                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return new Result<bool>(true, "customer_wishlist_deleted_successfully", true);
                        }
                        return new Result<bool>(false, "customer_wishlist_not_found", false, 404);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to delete customer Wishlist with id {id}", id);
                        return new Result<bool>(false, "internal_server_error", false, 500);
                    }
                }
            }
        }

        public async Task<Result<bool>> DeleteAsync(CustomerWishlistCreateDTO customerWishlist)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"DELETE FROM CustomerWishlist WHERE customer_id = @customerId AND product_id = @productId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@customerId", customerWishlist.CustomerId);
                    command.Parameters.AddWithValue("@productId", customerWishlist.ProductId);
                    try
                    {
                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return new Result<bool>(true, "customer_wishlist_deleted_successfully", true);
                        }
                        return new Result<bool>(false, "customer_wishlist_not_found", false, 404);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to delete customer Wishlist with customerId {customerId} and productId", customerWishlist.CustomerId, customerWishlist.ProductId);
                        return new Result<bool>(false, "internal_server_error", false, 500);
                    }
                }
            }
        }
    }
}
