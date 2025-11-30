using Jannara_Ecommerce.Dtos;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IPersonRepository
    {
        public Task<Result<PersonDTO>> AddNewAsync(PersonDTO newPerson);
        public Task<Result<bool>> UpdateAsync(int id, PersonDTO updatedPerson);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<PersonDTO>> GetById(int id);
    }
}
