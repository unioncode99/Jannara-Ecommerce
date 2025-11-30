using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IPersonRepository
    {
        public Task<Result<PersonDTO>> AddNewAsync(PersonDTO newPerson);
        public Task<Result<bool>> UpdateAsync(int id, PersonDTO updatedPerson);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<PersonDTO>> GetByIdAsync(int id);
    }
}
