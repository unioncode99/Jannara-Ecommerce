using Jannara_Ecommerce.Dtos;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface ICustomerRepository
    {
        public Task<Result<CustomerDTO>> GetByIdAsync(int id);
        public Task<Result<IEnumerable<CustomerDTO>>> GetAllAsync(int pageNumber = 1, int pageSize = 20);
        public Task<Result<CustomerDTO>> AddNewAsync(CustomerDTO newCustomer);
        public Task<Result<bool>> UpdateAsync(int id, CustomerDTO updatedCustomer);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
