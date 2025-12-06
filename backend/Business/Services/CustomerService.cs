using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.Enums;
using Jannara_Ecommerce.Mappers;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data.Common;

namespace Jannara_Ecommerce.Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;
        private readonly string _connectionString;
        private readonly IPersonService _personService;
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        public CustomerService(ICustomerRepository repo, IPersonService PersonService,
            IUserService UserService, IOptions<DatabaseSettings> options, IUserRoleService userRoleService)
        {
            _repo = repo;
            _connectionString = options.Value.DefaultConnection;
            _personService = PersonService;
            _userService = UserService;
            _userRoleService = userRoleService;
        }
        public async Task<Result<CustomerDTO>> AddNewAsync(int userId, SqlConnection connection, SqlTransaction transaction)
        {
            return await _repo.AddNewAsync(userId, connection, transaction);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<Result<CustomerDTO>> FindAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }
        public async Task<Result<CustomerDTO>> CreateAsync(CustomerCreateRequestDTO customerCreateRequestDTO)
        {

            var personCreateDTO = customerCreateRequestDTO.GetPersonCreateDTO();
            var userCreateDTO = customerCreateRequestDTO.GetUserCreateDTO(); 

            using (var connection = new SqlConnection(_connectionString))
            {
                DbTransaction transaction = null;
                try
                {
                    await connection.OpenAsync();
                    transaction = await connection.BeginTransactionAsync();
                    var personResult = await _personService.AddNewAsync(personCreateDTO, connection,(SqlTransaction) transaction);
                    if (!personResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return new Result<CustomerDTO>(false, personResult.Message, null, personResult.ErrorCode);
                    }
                    
                    var userResult = await _userService.AddNewAsync(personResult.Data.Id, userCreateDTO, connection, (SqlTransaction)transaction);
                    if (!userResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return new Result<CustomerDTO>(false, userResult.Message, null, userResult.ErrorCode);
                    }

                    var userRoleResult = await _userRoleService.AddNewAsync((int)Roles.Customer, userResult.Data.Id, true, connection, (SqlTransaction)transaction);
                    if (!userRoleResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return new Result<CustomerDTO>(false, userRoleResult.Message, null, userRoleResult.ErrorCode);
                    }

                    var customerResult = await AddNewAsync(userResult.Data.Id,  connection, (SqlTransaction)transaction);
                    if (!customerResult.IsSuccess)
                    {
                       await transaction.RollbackAsync();
                        return new Result<CustomerDTO>(false, customerResult.Message, null, customerResult.ErrorCode);
                    }
                    await transaction.CommitAsync();
                    return customerResult;
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        try
                        {
                            await transaction.RollbackAsync();
                        }
                        catch
                        {

                        }
                    }
                    return new Result<CustomerDTO>(false, "An unexpected error occurred on the server.", null, 500);
                }
            }
        }

        public async Task<Result<bool>> UpdateAsync(int id, CustomerDTO updatedCustomer)
        {
            return await _repo.UpdateAsync(id, updatedCustomer);
        }

        public Task<Result<PagedResponseDTO<CustomerDTO>>> GetAllAsync(int pageNumber, int pageSize)
        {
            return _repo.GetAllAsync(pageNumber, pageSize);
        }
    }
}
