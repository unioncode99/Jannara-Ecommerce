using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface ICustomerService
    {
        public Task<Result<CustomerDTO>> FindAsync(int id);
        public Task<Result<CustomerDTO>> AddNewAsync(CustomerDTO newCustomer, SqlConnection connection, SqlTransaction transaction);
        public Task<Result<bool>> UpdateAsync(int id, CustomerDTO updatedCustomer);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<CustomerDTO>> RegisterAsync(RegisteredCustomerDTO registeredCustomer);
    }
}
