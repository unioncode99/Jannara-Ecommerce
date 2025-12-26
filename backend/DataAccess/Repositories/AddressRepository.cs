using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Address;
using Jannara_Ecommerce.DTOs.Product;
using Jannara_Ecommerce.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;
using System.IO;
using System.Numerics;
using System.Text.Json;

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
state_id,
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
@state_id,
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
                    command.Parameters.AddWithValue("@state_id", addressCreateDTO.StateId);
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
                                    reader.GetInt32(reader.GetOrdinal("state_id")),
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

        public async Task<Result<AddressResponseDTO>> GetAllAsync(int personId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {


                string query = @"DECLARE @result NVARCHAR(MAX);

-- Generate JSON and store in variable
SET @result = (
    SELECT
        (SELECT 
            id AS Id,
            person_id AS PersonId,
            state_id AS StateId,
            city AS City,
            locality AS Locality,
            street AS Street,
            building_number AS BuildingNumber,
            phone AS Phone,
            created_at AS CreatedAt,
            updated_at AS UpdatedAt
         FROM addresses
         WHERE person_id = @person_id
         FOR JSON PATH) AS Addresses,

        (SELECT 
            id AS Id,
            code AS Code,
            name_en AS NameEn,
            name_ar AS NameAr,
            extra_fee_for_shipping AS ExtraFeeForShipping,
            created_at AS CreatedAt,
            updated_at AS UpdatedAt
         FROM states
         FOR JSON PATH) AS States
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
);

-- Return the JSON
SELECT @result AS JsonResult;";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@person_id", personId);

                    try
                    {
                        await connection.OpenAsync();
                        using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);
                        {
                            if (!await reader.ReadAsync())
                            {
                                return new Result<AddressResponseDTO>(false, "addresses_not_found", null, 404);
                            }

                            // Read the entire JSON as a string first
                            string json = await reader.GetFieldValueAsync<string>(0);

                            var address = JsonSerializer.Deserialize<AddressResponseDTO>(
                                json,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                            );

                            return new Result<AddressResponseDTO>(true, "addresses_retrieved_successfully", address, 200);

                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to get all addresses for PersonId {PersonId}", personId);
                        return new Result<AddressResponseDTO>(false, "internal_server_error", null, 500);
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
                                    reader.GetInt32(reader.GetOrdinal("state_id")),
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
    state_id = @state_id,
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
                    command.Parameters.AddWithValue("@state_id", addressUpdateDTO.StateId);
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
