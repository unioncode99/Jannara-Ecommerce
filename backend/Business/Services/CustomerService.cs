using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;
        private readonly string _connectionString;
        private readonly IPersonService _personService;
        private readonly IUserService _userService;
        public CustomerService(ICustomerRepository repo, IPersonService PersonService, IUserService UserService, IOptions<DatabaseSettings> options)
        {
            _repo = repo;
            _connectionString = options.Value.DefaultConnection;
            _personService = PersonService;
            _userService = UserService;
        }
        public async Task<Result<CustomerDTO>> AddNewAsync(CustomerDTO newCustomer, SqlConnection connection, SqlTransaction transaction)
        {
            return await _repo.AddNewAsync(newCustomer, connection, transaction);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<Result<CustomerDTO>> FindAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }
        public async Task<Result<CustomerDTO>> RegisterAsync(RegisteredCustomerDTO registeredCustomer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    await connection.OpenAsync();
                    transaction = connection.BeginTransaction();
                    Result<PersonDTO> PersonResult = await _personService.AddNewAsync(registeredCustomer.Person, connection, transaction);
                    if (!PersonResult.IsSuccess)
                    {
                        transaction.Rollback();
                        return new Result<CustomerDTO>(false, PersonResult.Message, null, PersonResult.ErrorCode);
                    }
                    Result<UserPublicDTO> UserResult = await _userService.AddNewAsync(registeredCustomer.User, connection, transaction);
                    if (!UserResult.IsSuccess)
                    {
                        transaction.Rollback();
                        return new Result<CustomerDTO>(false, UserResult.Message, null, UserResult.ErrorCode);
                    }
                    CustomerDTO CustomerInfo = new CustomerDTO(-1, UserResult.Data.Id, DateTime.Now, DateTime.Now);
                    Result<CustomerDTO> CustomerResult = await AddNewAsync(CustomerInfo, connection, transaction);
                    if (!CustomerResult.IsSuccess)
                    {
                        transaction.Rollback();
                        return new Result<CustomerDTO>(false, CustomerResult.Message, null, CustomerResult.ErrorCode);
                    }
                    transaction.Commit();
                    return CustomerResult;
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    return new Result<CustomerDTO>(false, "An unexpected error occurred on the server.", null, 500);
                }
            }
        }

        public async Task<Result<bool>> UpdateAsync(int id, CustomerDTO updatedCustomer)
        {
            return await _repo.UpdateAsync(id, updatedCustomer);
        }
    }
}
