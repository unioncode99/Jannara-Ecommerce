using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IPersonService
    {
        public Task<Result<PersonDTO>> FindAsync(int id);
        public Task<Result<PersonDTO>> AddNewAsync(PersonCreateDTO personCreateDTO, string imageUrl, SqlConnection connection, SqlTransaction transaction);
        public Task<Result<PersonDTO>> UpdateAsync(int id, PersonUpdateDTO updatedPerson);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
