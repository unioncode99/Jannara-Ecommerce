using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IPersonRepository
    {
        public Task<Result<PersonDTO>> AddNewAsync(PersonDTO newPerson, SqlConnection connection, SqlTransaction transaction);
        public Task<Result<bool>> UpdateAsync(int id, PersonDTO updatedPerson);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<PersonDTO>> GetByIdAsync(int id);
    }
}
