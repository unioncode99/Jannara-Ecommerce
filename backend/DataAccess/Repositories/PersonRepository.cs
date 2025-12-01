using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Enums;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Security.Principal;

namespace Jannara_Ecommerce.DataAccess.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly string _connectionString;
        public PersonRepository(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.DefaultConnection;
        }
        public async Task<Result<PersonDTO>> AddNewAsync(PersonDTO newPerson, SqlConnection connection, SqlTransaction transaction)
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
 VALUES(
@first_name,
@last_name,
@phone,
@image_url,
@gender,
@date_of_birth
);
Select * from People Where Id  = (SELECT SCOPE_IDENTITY());
";
            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@first_name", newPerson.FirstName);
                command.Parameters.AddWithValue("@last_name", newPerson.LastName);
                command.Parameters.AddWithValue("@phone", newPerson.Phone);
                command.Parameters.AddWithValue("@image_url", newPerson.ImageUrl ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@gender", (int)newPerson.Gender);
                command.Parameters.AddWithValue("@date_of_birth", newPerson.DateOfBirth);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        PersonDTO insertedPerson = new PersonDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetString(reader.GetOrdinal("first_name")),
                            reader.GetString(reader.GetOrdinal("last_name")),
                            reader.GetString(reader.GetOrdinal("phone")),
                            reader.IsDBNull(reader.GetOrdinal("image_url")) ? null : reader.GetString(reader.GetOrdinal("image_url")),
                            (Gender)reader.GetInt32(reader.GetOrdinal("gender")),
                            DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_of_birth"))),
                            reader.GetDateTime(reader.GetOrdinal("created_at")),
                            reader.GetDateTime(reader.GetOrdinal("updated_at"))
                       );
                        return new Result<PersonDTO>(true, "Person added successfully.", insertedPerson);
                    }
                    return new Result<PersonDTO>(false, "Failed to add person.", null, 500);

                }

            }
        }
        public async Task<Result<bool>> DeleteAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM People WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                            return new Result<bool>(true, "Person deleted successfully.", true);
                        return new Result<bool>(false, "Person not found.", false, 500);
                    }
                    catch (Exception ex)
                    {
                        return new Result<bool>(false, "An unexpected error occurred on the server.", false, 500);
                    }

                }
            }
        }
        public async Task<Result<PersonDTO>> GetByIdAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
Select * from People Where Id  = @id;
";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
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
                                    (Gender)reader.GetInt32(reader.GetOrdinal("gender")),
                                    DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_of_birth"))),
                                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                                    reader.GetDateTime(reader.GetOrdinal("updated_at"))
                               );
                                return new Result<PersonDTO>(true, "Person retrieved successfully.", person);
                            }
                            return new Result<PersonDTO>(false, "Person not found.", null, 404);

                        }


                    }
                    catch (Exception ex)
                    {
                        return new Result<PersonDTO>(false, "An unexpected error occurred on the server.", null, 500);
                    }

                }
            }
        }
        public async Task<Result<bool>> UpdateAsync(int id, PersonDTO updatedPerson)
        {
            using(SqlConnection connection = new SqlConnection(_connectionString))
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
select @@ROWCOUNT";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@first_name", updatedPerson.FirstName);
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@last_name", updatedPerson.LastName);
                    command.Parameters.AddWithValue("@phone", updatedPerson.Phone);
                    command.Parameters.AddWithValue("@image_url", updatedPerson.ImageUrl ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@gender", (int)updatedPerson.Gender);
                    command.Parameters.AddWithValue("@date_of_birth", updatedPerson.DateOfBirth);


                    try
                    {
                        await connection.OpenAsync();
                        object? result = await command.ExecuteScalarAsync();
                        int rowAffected = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        if (rowAffected > 0)
                            return new Result<bool>(true, "Person updated successfully.", true);
                        return new Result<bool>(false, "Person not found.", false, 404);
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
