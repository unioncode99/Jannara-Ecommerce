using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.Dtos.Person;
using Jannara_Ecommerce.Dtos.User;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.Mappers;
using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Enums;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.Business.Services
{
    public class SellerService : ISellerService
    {
        private readonly ISellerRepository _repo;
        private readonly string _connectionString;
        private readonly IPersonService _personService;
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        public SellerService(ISellerRepository repo, IPersonService PersonService, 
            IUserService UserService, IOptions<DatabaseSettings> options, IUserRoleService userRoleService)
        {
            _repo  = repo;
            _connectionString = options.Value.DefaultConnection;
            _personService = PersonService;
            _userService = UserService;
            _userRoleService = userRoleService;
        }
        public async Task<Result<SellerDTO>> AddNewAsync(int userId, SellerCreateDTO newSeller, SqlConnection connection, SqlTransaction transaction)
        {
            return await _repo.AddNewAsync(userId, newSeller, connection, transaction);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<Result<SellerDTO>> FindAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }
        public async Task<Result<SellerDTO>> CreateAsync(SellerCreateRequestDTO sellerCreateRequestDTO)
        {
            PersonCreateDTO newPerson = sellerCreateRequestDTO.GetPersonCreateDTO();
            UserCreateDTO newUser = sellerCreateRequestDTO.GetUserCreateDTO();
            SellerCreateDTO newSeller = sellerCreateRequestDTO.GetSellerCreateDTO();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    await connection.OpenAsync();
                    transaction = connection.BeginTransaction();
                    Result<PersonDTO> personResult = await _personService.AddNewAsync(newPerson, connection, transaction);
                    if (!personResult.IsSuccess)
                    {
                        transaction.Rollback();
                        return new Result<SellerDTO>(false, personResult.Message, null, personResult.ErrorCode);
                    }
                    Result<UserPublicDTO> userResult = await _userService.AddNewAsync(personResult.Data.Id, newUser, connection, transaction);
                    if (!userResult.IsSuccess)
                    {
                        transaction.Rollback();
                        return new Result<SellerDTO>(false, userResult.Message, null, userResult.ErrorCode);
                    }

                    Result<UserRoleDTO> userRoleResult = await _userRoleService.AddNewAsync((int)Roles.Seller, userResult.Data.Id, true, connection, transaction);
                    if (!userRoleResult.IsSuccess)
                    {
                        transaction.Rollback();
                        return new Result<SellerDTO>(false, userResult.Message, null, userResult.ErrorCode);
                    }
                    Result<SellerDTO> sellerResult = await AddNewAsync (userResult.Data.Id, newSeller, connection, transaction);
                    if (!sellerResult.IsSuccess)
                    {
                        transaction.Rollback();
                        return new Result<SellerDTO>(false, sellerResult.Message, null, sellerResult.ErrorCode);
                    }
                    transaction.Commit();
                    return sellerResult;
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    return new Result<SellerDTO>(false, "An unexpected error occurred on the server.", null, 500);
                }
            }
        }
        public async Task<Result<bool>> UpdateAsync(int id, SellerUpdateDTO updatedSeller)
        {
            return await _repo.UpdateAsync(id, updatedSeller);
        }
    }
}
