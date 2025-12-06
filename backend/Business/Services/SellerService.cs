using Jannara_Ecommerce.Business.DTOs.Mappers;
using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.Business.Mappers;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.Dtos.Person;
using Jannara_Ecommerce.Dtos.User;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Mappers;
using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Enums;
using Jannara_Ecommerce.Mappers;
using Jannara_Ecommerce.Properties.Mappers;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data.Common;

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
            var newPerson = sellerCreateRequestDTO.GetPersonCreateDTO();
            var newUser = sellerCreateRequestDTO.GetUserCreateDTO();
            var newSeller = sellerCreateRequestDTO.GetSellerCreateDTO();
            using (var connection = new SqlConnection(_connectionString))
            {
                DbTransaction transaction = null;
                try
                {
                    await connection.OpenAsync();
                    transaction = await connection.BeginTransactionAsync();
                    var personResult = await _personService.AddNewAsync(newPerson, connection,(SqlTransaction) transaction);
                    if (!personResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return new Result<SellerDTO>(false, personResult.Message, null, personResult.ErrorCode);
                    }
                    var userResult = await _userService.AddNewAsync(personResult.Data.Id, newUser, connection, (SqlTransaction)transaction);
                    if (!userResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return new Result<SellerDTO>(false, userResult.Message, null, userResult.ErrorCode);
                    }

                    var userRoleResult = await _userRoleService.AddNewAsync((int)Roles.Seller, userResult.Data.Id, true, connection, (SqlTransaction)transaction);
                    if (!userRoleResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return new Result<SellerDTO>(false, userResult.Message, null, userResult.ErrorCode);
                    }
                    var sellerResult = await AddNewAsync (userResult.Data.Id, newSeller, connection, (SqlTransaction)transaction);
                    if (!sellerResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return new Result<SellerDTO>(false, sellerResult.Message, null, sellerResult.ErrorCode);
                    }
                    await transaction.CommitAsync();
                    return sellerResult;
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
                    return new Result<SellerDTO>(false, "An unexpected error occurred on the server.", null, 500);
                }
            }
        }
        public async Task<Result<bool>> UpdateAsync(int id, SellerUpdateDTO updatedSeller)
        {
            return await _repo.UpdateAsync(id, updatedSeller);
        }


        public Task<Result<PagedResponseDTO<SellerDTO>>> GetAllAsync(int pageNumber, int pageSize)
        {
            return _repo.GetAllAsync(pageNumber, pageSize);
        }
    }
}
