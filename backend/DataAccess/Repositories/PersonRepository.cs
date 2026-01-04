using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.Enums;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<IPersonRepository> _logger;
        public PersonRepository(IOptions<DatabaseSettings> options, ILogger<IPersonRepository> logger)
        {
            _connectionString = options.Value.DefaultConnection;
            _logger = logger;
        }
        public async Task<Result<PersonDTO>> AddNewAsync(PersonCreateDTO  personCreateDTO, string imageUrl, SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"

INSERT INTO People
           (
first_name,
last_name,
phone,
image_url,
gender,
date_of_birth)
OUTPUT inserted.*
 VALUES(
@first_name,
@last_name,
@phone,
@image_url,
@gender,
@date_of_birth
);
";
            using (var command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@first_name", personCreateDTO.FirstName);
                command.Parameters.AddWithValue("@last_name", personCreateDTO.LastName);
                command.Parameters.AddWithValue("@phone", personCreateDTO.Phone);
                command.Parameters.AddWithValue("@image_url", imageUrl ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@gender", (int)personCreateDTO.Gender);
                command.Parameters.AddWithValue("@date_of_birth", personCreateDTO.DateOfBirth);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var insertedPerson = new PersonDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetString(reader.GetOrdinal("first_name")),
                            reader.GetString(reader.GetOrdinal("last_name")),
                            reader.GetString(reader.GetOrdinal("phone")),
                            reader.IsDBNull(reader.GetOrdinal("image_url")) ? null : reader.GetString(reader.GetOrdinal("image_url")),
                            (Gender)reader.GetByte(reader.GetOrdinal("gender")),
                            DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_of_birth"))),
                            reader.GetDateTime(reader.GetOrdinal("created_at")),
                            reader.GetDateTime(reader.GetOrdinal("updated_at"))
                       );
                        return new Result<PersonDTO>(true, "person_added_successfully", insertedPerson);
                    }
                    return new Result<PersonDTO>(false, "failed_to_add_person", null, 500);

                }

            }
        }
        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM People WHERE Id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                            return new Result<bool>(true, "person_deleted_successfully", true);
                        return new Result<bool>(false, "person_not_found", false, 500);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to delete person with PersonId {PersonId}", id);
                        return new Result<bool>(false, "internal_server_error", false, 500);
                    }

                }
            }
        }
        public async Task<Result<PersonDTO>> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
Select * from People Where Id  = @id;
";
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
                                PersonDTO person = new PersonDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("first_name")),
                                    reader.GetString(reader.GetOrdinal("last_name")),
                                    reader.GetString(reader.GetOrdinal("phone")),
                                    reader.IsDBNull(reader.GetOrdinal("image_url")) ? null : reader.GetString(reader.GetOrdinal("image_url")),
                                    (Gender)reader.GetByte(reader.GetOrdinal("gender")),
                                    DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_of_birth"))),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                               );
                                return new Result<PersonDTO>(true, "person_retrieved_successfully", person);
                            }
                            return new Result<PersonDTO>(false, "person_not_found", null, 404);

                        }


                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve person with PersonId {PersonId}", id);
                        return new Result<PersonDTO>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }
        public async Task<Result<PersonDTO>> UpdateAsync(int id, PersonUpdateDTO  updatedPerson, string imageUrl)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                string query = @"
UPDATE People
SET 
    first_name = @first_name,
    last_name = @last_name, 
    phone = @phone,
    image_url = @image_url,
    gender = @gender,
    date_of_birth = @date_of_birth
WHERE Id = @id;
select * from People where id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@first_name", updatedPerson.FirstName);
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@last_name", updatedPerson.LastName);
                    command.Parameters.AddWithValue("@phone", updatedPerson.Phone);
                    command.Parameters.AddWithValue("@image_url", imageUrl ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@gender", (int)updatedPerson.Gender);
                    command.Parameters.AddWithValue("@date_of_birth", updatedPerson.DateOfBirth);

                    try
                    {
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                PersonDTO person = new PersonDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("first_name")),
                                    reader.GetString(reader.GetOrdinal("last_name")),
                                    reader.GetString(reader.GetOrdinal("phone")),
                                    reader.IsDBNull(reader.GetOrdinal("image_url")) ? null : reader.GetString(reader.GetOrdinal("image_url")),
                                    (Gender)reader.GetByte(reader.GetOrdinal("gender")),
                                    DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_of_birth"))),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                               );
                                return new Result<PersonDTO>(true, "person_updated_successfully", person);
                            }
                            return new Result<PersonDTO>(false, "person_not_found", null, 404);

                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to update person with PersonId {PersonId}", id);
                        return new Result<PersonDTO>(false, "internal_server_error", null, 500);
                    }

                }
            }
        }
    }
}
