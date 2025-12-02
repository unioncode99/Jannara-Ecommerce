using Jannara_Ecommerce.Dtos.Customer;
using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface ICustomerService
    {
        public Task<Result<CustomerDTO>> FindAsync(int id);
        public Task<Result<CustomerDTO>> AddNewAsync(int userId, SqlConnection connection, SqlTransaction transaction);
        public Task<Result<bool>> UpdateAsync(int id, CustomerDTO updatedCustomer);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<CustomerDTO>> CreateAsync(CustomerCreateRequestDTO customerCreateRequestDTO);
    }
}
