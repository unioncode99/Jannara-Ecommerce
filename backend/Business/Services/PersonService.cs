using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repo;
        private readonly IWebHostEnvironment _env;

        public PersonService(IPersonRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }
        public async Task<Result<PersonDTO>> AddNewAsync(PersonDTO newAccount)
        {
            return await _repo.AddNewAsync(newAccount);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<Result<PersonDTO>> FindAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Result<bool>> UpdateAsync(int id, PersonDTO updatedAccount)
        {
           return await _repo.UpdateAsync(id, updatedAccount);
        }

       
    }
}
