using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repo;
        public PersonService(IPersonRepository repo)
        {
            _repo = repo;
        }
        public async Task<Result<PersonDTO>> AddNewAsync(PersonDTO newAccount, SqlConnection connection, SqlTransaction transaction)
        {
            return await _repo.AddNewAsync(newAccount, connection, transaction);
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
