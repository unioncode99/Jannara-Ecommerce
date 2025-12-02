using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.Dtos;
using Jannara_Ecommerce.Dtos.Customer;
using Jannara_Ecommerce.Dtos.Mappers;
using Jannara_Ecommerce.Dtos.Person;
using Jannara_Ecommerce.Dtos.User;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Enums;
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

            PersonCreateDTO personCreateDTO = customerCreateRequestDTO.GetPersonCreateDTO();
            UserCreateDTO userCreateDTO = customerCreateRequestDTO.GetUserCreateDTO(); 

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    await connection.OpenAsync();
                    transaction = connection.BeginTransaction();
                    Result<PersonDTO> personResult = await _personService.AddNewAsync(personCreateDTO, connection, transaction);
                    if (!personResult.IsSuccess)
                    {
                        transaction.Rollback();
                        return new Result<CustomerDTO>(false, personResult.Message, null, personResult.ErrorCode);
                    }
                    
                    Result<UserPublicDTO> userResult = await _userService.AddNewAsync(personResult.Data.Id, userCreateDTO, connection, transaction);
                    if (!userResult.IsSuccess)
                    {
                        transaction.Rollback();
                        return new Result<CustomerDTO>(false, userResult.Message, null, userResult.ErrorCode);
                    }

                    Result<UserRoleDTO> userRoleResult = await _userRoleService.AddNewAsync((int)Roles.Customer, userResult.Data.Id, true, connection, transaction);
                    if (!userRoleResult.IsSuccess)
                    {
                        transaction.Rollback();
                        return new Result<CustomerDTO>(false, userResult.Message, null, userResult.ErrorCode);
                    }

                    Result<CustomerDTO> customerResult = await AddNewAsync(userResult.Data.Id,  connection, transaction);
                    if (!customerResult.IsSuccess)
                    {
                        transaction.Rollback();
                        return new Result<CustomerDTO>(false, customerResult.Message, null, customerResult.ErrorCode);
                    }
                    transaction.Commit();
                    return customerResult;
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
