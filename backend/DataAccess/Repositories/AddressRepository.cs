using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Address;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<IAddressRepository> _logger;
        public AddressRepository(IOptions<DatabaseSettings> options, ILogger<IAddressRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }
        public async Task<Result<AddressDTO>> AddNewAsync(AddressCreateDTO addressCreateDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO Addresses
(
person_id,
state,
city,
locality,
street,
building_number,
phone
)
OUTPUT inserted.*
VALUES
(
@person_id,
@state,
@city,
@locality,
@street,
@building_number,
@phone
);
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@person_id", addressCreateDTO.PersonId);
                    command.Parameters.AddWithValue("@state", addressCreateDTO.State);
                    command.Parameters.AddWithValue("@city", addressCreateDTO.City);
                    command.Parameters.AddWithValue("@locality", addressCreateDTO.Locality);
                    command.Parameters.AddWithValue("@street", addressCreateDTO.Street);
                    command.Parameters.AddWithValue("@building_number", addressCreateDTO.BuildingNumber);
                    command.Parameters.AddWithValue("@phone", addressCreateDTO.Phone ?? (object)DBNull.Value);
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                AddressDTO insertedAddress = new AddressDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetInt32(reader.GetOrdinal("person_id")),
                                    reader.GetString(reader.GetOrdinal("state")),
                                    reader.GetString(reader.GetOrdinal("city")),
                                    reader.GetString(reader.GetOrdinal("locality")),
                                    reader.GetString(reader.GetOrdinal("street")),
                                    reader.GetString(reader.GetOrdinal("building_number")),
                                    reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                );
                                return new Result<AddressDTO>(true, "address_added_successfully", insertedAddress);
                            }
                            return new Result<AddressDTO>(false, "failed_to_add_address", null, 500);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to add new address");
                        return new Result<AddressDTO>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Addresses WHERE id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    try
                    {
                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return new Result<bool>(true, "address_deleted_successfully", true);
                        }
                        return new Result<bool>(false, "address_not_found", false, 404);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to delete address with AddressId {AddressId}", id);
                        return new Result<bool>(false, "internal_server_error", false, 500);
                    }
                }
            }
        }

        public async Task<Result<IEnumerable<AddressDTO>>> GetAllAsync(int personId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"Select * from Addresses Where person_id  = @person_id;";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@person_id", personId);
                    var personAddresses = new List<AddressDTO>();
                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                personAddresses.Add(new AddressDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetInt32(reader.GetOrdinal("person_id")),
                                    reader.GetString(reader.GetOrdinal("state")),
                                    reader.GetString(reader.GetOrdinal("city")),
                                    reader.GetString(reader.GetOrdinal("locality")),
                                    reader.GetString(reader.GetOrdinal("street")),
                                    reader.GetString(reader.GetOrdinal("building_number")),
                                    reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                ));
                            }
                            if (personAddresses.Count > 0)
                            {
                                return new Result<IEnumerable<AddressDTO>>(true, "addresses_retrieved_successfully", personAddresses);
                            }
                            return new Result<IEnumerable<AddressDTO>>(false, "addresses_not_found", null, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to get all addresses for PersonId {PersonId}", personId);
                        return new Result<IEnumerable<AddressDTO>>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }

        public async Task<Result<AddressDTO>> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"Select * from Addresses Where id  = @id;";
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
                                AddressDTO address = new AddressDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("id")),
                                    reader.GetInt32(reader.GetOrdinal("person_id")),
                                    reader.GetString(reader.GetOrdinal("state")),
                                    reader.GetString(reader.GetOrdinal("city")),
                                    reader.GetString(reader.GetOrdinal("locality")),
                                    reader.GetString(reader.GetOrdinal("street")),
                                    reader.GetString(reader.GetOrdinal("building_number")),
                                    reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                );
                                return new Result<AddressDTO>(true, "address_retrieved_successfully", address);
                            }
                            return new Result<AddressDTO>(false, "address_not_found", null, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve information for AddressId {AddressId}", id);
                        return new Result<AddressDTO>(false, "internal_server_error", null, 500);
                    }
                }
            }
        }

        public async Task<Result<bool>> UpdateAsync(int id, AddressUpdateDTO addressUpdateDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE Addresses
SET 
    state = @state,
    city = @city,
    locality = @locality,
    street = @street,
    building_number = @building_number,
    phone = @phone
WHERE id = @id;
select @@ROWCOUNT
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@state", addressUpdateDTO.State);
                    command.Parameters.AddWithValue("@city", addressUpdateDTO.City);
                    command.Parameters.AddWithValue("@locality", addressUpdateDTO.Locality);
                    command.Parameters.AddWithValue("@street", addressUpdateDTO.Street);
                    command.Parameters.AddWithValue("@building_number", addressUpdateDTO.BuildingNumber);
                    command.Parameters.AddWithValue("@phone", addressUpdateDTO.City ?? (object)DBNull.Value);
                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowsAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowsAffected > 0)
                        {
                            return new Result<bool>(true, "address_updated_successfully", true);
                        }
                        return new Result<bool>(false, "address_not_found", false, 404);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to update address for AddressId {AddressId}", id);
                        return new Result<bool>(false, "internal_server_error", false, 500);
                    }
                }
            }
        }
    }
}
