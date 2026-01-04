using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IPersonRepository
    {
        public Task<Result<PersonDTO>> AddNewAsync(PersonCreateDTO personCreateDTO, string imageUrl, SqlConnection connection, SqlTransaction transaction);
        public Task<Result<PersonDTO>> UpdateAsync(int id, PersonUpdateDTO updatedPerson, string imageUrl);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<PersonDTO>> GetByIdAsync(int id);
    }
}
