using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly string _connectionString;
        public AddressRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public async Task<Result<AddressDTO>> AddNewAsync(AddressDTO newAddress)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
INSERT INTO Addresses
(
person_id,
street,
city,
state
)
OUTPUT inserted.*
VALUES
(
@person_id,
@street,
@city,
@state
);
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@person_id", newAddress.PersonId);
                    command.Parameters.AddWithValue("@street", newAddress.Street);
                    command.Parameters.AddWithValue("@city", newAddress.City);
                    command.Parameters.AddWithValue("@state", newAddress.State ?? (object)DBNull.Value);
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
                                    reader.GetString(reader.GetOrdinal("street")),
                                    reader.GetString(reader.GetOrdinal("city")),
                                    reader.IsDBNull(reader.GetOrdinal("state")) ? null : reader.GetString(reader.GetOrdinal("state")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                );
                                return new Result<AddressDTO>(true, "Address added successfully.", insertedAddress);
                            }
                            return new Result<AddressDTO>(false, "Failed To Add Address", null, 500);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<AddressDTO>(false, "An unexpected error occurred on the server.", null, 500);
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
                        object? result = await command.ExecuteScalarAsync();
                        int rowsAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowsAffected > 0)
                        {
                            return new Result<bool>(true, "Address deleted successfully.", true);
                        }
                        return new Result<bool>(false, "Role Not Found", false, 404);
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }
                }
            }
        }

        public async Task<Result<IEnumerable<AddressDTO>>> GetAllAsync(int person_id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"Select * from Addresses Where person_id  = @person_id;";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@person_id", person_id);
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
                                    reader.GetString(reader.GetOrdinal("street")),
                                    reader.GetString(reader.GetOrdinal("city")),
                                    reader.IsDBNull(reader.GetOrdinal("state")) ? null : reader.GetString(reader.GetOrdinal("state")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                ));
                            }
                            if (personAddresses.Count > 0)
                            {
                                return new Result<IEnumerable<AddressDTO>>(true, "Addresses retrieved successfully", personAddresses);
                            }
                            return new Result<IEnumerable<AddressDTO>>(false, "Addresses Not Found", null, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<IEnumerable<AddressDTO>>(false, "An unexpected error occurred on the server.", null, 500);
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
                                    reader.GetString(reader.GetOrdinal("street")),
                                    reader.GetString(reader.GetOrdinal("city")),
                                    reader.IsDBNull(reader.GetOrdinal("state")) ? null : reader.GetString(reader.GetOrdinal("state")),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                                );
                                return new Result<AddressDTO>(true, "Address retrieved successfully.", address);
                            }
                            return new Result<AddressDTO>(false, "Address Not Found", null, 404);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<AddressDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }
                }
            }
        }

        public async Task<Result<bool>> UpdateAsync(int id, AddressDTO updatedAddress)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE Addresses
SET 
    street = @street,
    city = @city,
    state = @state
WHERE id = @id;
select @@ROWCOUNT
";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@street", updatedAddress.Street);
                    command.Parameters.AddWithValue("@city", updatedAddress.City);
                    command.Parameters.AddWithValue("@state", updatedAddress.State ?? (object)DBNull.Value);
                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowsAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowsAffected > 0)
                        {
                            return new Result<bool>(true, "Address updated successfully.", true);
                        }
                        return new Result<bool>(false, "Address Not Found.", false, 404);
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }
                }
            }
        }
    }
}
