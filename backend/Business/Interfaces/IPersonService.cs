using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IPersonService
    {
        public Task<Result<PersonDTO>> FindAsync(int id);
        public Task<Result<PersonDTO>> AddNewAsync(PersonDTO newPerson, SqlConnection connection, SqlTransaction transaction);
        public Task<Result<bool>> UpdateAsync(int id, PersonDTO updatedPerson);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
