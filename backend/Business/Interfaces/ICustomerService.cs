using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface ICustomerService
    {
        public Task<Result<CustomerDTO>> FindAsync(int id);
        public Task<Result<CustomerDTO>> AddNewAsync(CustomerDTO newCustomer);
        public Task<Result<bool>> UpdateAsync(int id, CustomerDTO updatedCustomer);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
