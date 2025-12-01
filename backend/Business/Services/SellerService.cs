using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.DTOs.Mapper;
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
        public SellerService(ISellerRepository repo, IPersonService PersonService, 
            IUserService UserService, IOptions<DatabaseSettings> options)
        {
            _repo  = repo;
            _connectionString = options.Value.DefaultConnection;
            _personService = PersonService;
            _userService = UserService;
        }
        public async Task<Result<SellerDTO>> AddNewAsync(SellerDTO newSeller, SqlConnection connection, SqlTransaction transaction)
        {
            return await _repo.AddNewAsync(newSeller, connection, transaction);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<Result<SellerDTO>> FindAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }
        public async Task<Result<SellerDTO>> RegisterAsync(RegisteredSellerDTO registeredSeller)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    await connection.OpenAsync();
                    transaction = connection.BeginTransaction();
                    PersonDTO PersonInfo = registeredSeller.GetPersonDTO();
                    Result<PersonDTO> PersonResult = await _personService.AddNewAsync(PersonInfo, connection, transaction);
                    if (!PersonResult.IsSuccess)
                    {
                        transaction.Rollback();
                        return new Result<SellerDTO>(false, PersonResult.Message, null, PersonResult.ErrorCode);
                    }
                    UserDTO UserInfo = registeredSeller.GetUserDTO(PersonResult.Data.Id);
                    Result<UserPublicDTO> UserResult = await _userService.AddNewAsync(UserInfo, connection, transaction);
                    if (!UserResult.IsSuccess)
                    {
                        transaction.Rollback();
                        return new Result<SellerDTO>(false, UserResult.Message, null, UserResult.ErrorCode);
                    }
                    SellerDTO SellerInfo = new SellerDTO(-1, UserInfo.Id, registeredSeller.BusinessName, registeredSeller.WebsiteUrl, DateTime.Now, DateTime.Now);
                    Result<SellerDTO> SellerResult = await AddNewAsync(SellerInfo, connection, transaction);
                    if (!SellerResult.IsSuccess)
                    {
                        transaction.Rollback();
                        return new Result<SellerDTO>(false, SellerResult.Message, null, SellerResult.ErrorCode);
                    }
                    transaction.Commit();
                    return SellerResult;
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    return new Result<SellerDTO>(false, "An unexpected error occurred on the server.", null, 500);
                }
            }
        }
        public async Task<Result<bool>> UpdateAsync(int id, SellerDTO updatedSeller)
        {
            return await _repo.UpdateAsync(id, updatedSeller);
        }
    }
}
