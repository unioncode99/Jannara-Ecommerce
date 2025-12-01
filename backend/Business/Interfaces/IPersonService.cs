using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IPersonService
    {
        public Task<Result<PersonDTO>> FindAsync(int id);
        public Task<Result<string>> _handleSaveImage(IFormFile image);
        public Task<Result<PersonDTO>> AddNewAsync(PersonDTO newPerson);
        public Task<Result<bool>> UpdateAsync(int id, PersonDTO updatedPerson);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
