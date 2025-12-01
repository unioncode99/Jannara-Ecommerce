using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IPasswordService _passwordService;
        private readonly string _connectionString;
        private readonly IPersonService _personService;
        public UserService(IUserRepository repo, IPasswordService passwordService,
            IOptions<DatabaseSettings> options, IPersonService personService)
        {
            _repo = repo;
            _passwordService = passwordService;
            _connectionString = options.Value.DefaultConnection;
            _personService = personService;
        }
        public async Task<Result<UserPublicDTO>> AddNewAsync(UserDTO newUser, SqlConnection connection, SqlTransaction transaction)
        {
            Result<UserDTO> findResult = await FindAsync(newUser.Email);
            if (!findResult.IsSuccess)
                return new Result<UserPublicDTO>(false, "This email is already registerd", null, 409);
            newUser.Password = _passwordService.HashPassword(newUser);
            return await _repo.AddNewAsync(newUser, connection, transaction);
        }

        public async Task<Result<UserPublicDTO>> CreateAsync(CreateNewUserDTO createUserDTO)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    await connection.OpenAsync();
                    transaction = connection.BeginTransaction();
                    Result<PersonDTO> personResult = await _personService.AddNewAsync(createUserDTO.Person, connection, transaction);
                    if (!personResult.IsSuccess)
                    {
                        transaction.Rollback();
                        return new Result<UserPublicDTO>(false, personResult.Message, null, personResult.ErrorCode);
                    }
                    Result<UserPublicDTO> userResult = await AddNewAsync(createUserDTO.User, connection, transaction);
                    if (!userResult.IsSuccess)
                    {
                        transaction.Rollback();
                        return new Result<UserPublicDTO>(false, userResult.Message, null, userResult.ErrorCode);
                    }
                    
                    transaction.Commit();
                    return userResult;
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    return new Result<UserPublicDTO>(false, "An unexpected error occurred on the server.", null, 500);
                }
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<Result<UserDTO>> FindAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Result<UserDTO>> FindAsync(string email)
        {
            return await _repo.GetByEmailAsync(email);
        }

        public async Task<Result<bool>> UpdateAsync(int id, UserDTO updatedUser)
        {
            Result<UserDTO> existingUser = await _repo.GetByEmailAsync(updatedUser.Email);
            if (!existingUser.IsSuccess && existingUser.ErrorCode != 404)
                return new Result<bool>(false, existingUser.Message, false, existingUser.ErrorCode);

            if (existingUser.IsSuccess && existingUser.Data.Id != id)
                return new Result<bool>(false, "This email is already register!", false, 409);

            updatedUser.Password = _passwordService.HashPassword(updatedUser);
            return await _repo.UpdateAsync(id, updatedUser);
        }
    }
}
