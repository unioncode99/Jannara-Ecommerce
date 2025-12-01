using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IPersonService
    {
        public Task<Result<PersonDTO>> FindAsync(int id);
<<<<<<< HEAD
        public Task<Result<string>> _handleSaveImage(IFormFile image);
        public Task<Result<PersonDTO>> AddNewAsync(PersonDTO newPerson);
=======
        public Task<Result<PersonDTO>> AddNewAsync(PersonDTO newPerson, SqlConnection connection, SqlTransaction transaction);
>>>>>>> f48aaceaaf88bf610a255315ee8c037823d380ac
        public Task<Result<bool>> UpdateAsync(int id, PersonDTO updatedPerson);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
