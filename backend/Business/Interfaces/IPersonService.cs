using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IPersonService
    {
        public Task<Result<PersonDTO>> FindAsync(int id);
        public Task<Result<PersonDTO>> AddNewAsync(PersonDTO newAccount);
        public Task<Result<bool>> UpdateAsync(int id, PersonDTO updatedAccount);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
