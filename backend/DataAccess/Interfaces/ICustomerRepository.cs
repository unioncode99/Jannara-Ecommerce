using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface ICustomerRepository
    {
        public Task<Result<CustomerDTO>> GetByIdAsync(int id);
        public Task<Result<PagedResponseDTO<CustomerDTO>>> GetAllAsync(int pageNumber = 1, int pageSize = 20);
        public Task<Result<CustomerDTO>> AddNewAsync(int userId, SqlConnection connection, SqlTransaction transaction);
        public Task<Result<bool>> UpdateAsync(int id, CustomerDTO updatedCustomer);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
