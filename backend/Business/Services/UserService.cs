using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Enums;
using Jannara_Ecommerce.Mappers;
using Jannara_Ecommerce.Utilities;
using Jannara_Ecommerce.Utilities.WrapperClasses;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data.Common;
namespace Jannara_Ecommerce.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IPasswordService _passwordService;
        private readonly string _connectionString;
        private readonly IPersonService _personService;
        private readonly IUserRoleService _userRoleService;
        private readonly IConfirmationService _accountConfirmationService;
        private readonly IImageService _imageService;
        private readonly IOptions<ImageSettings> _imageSettings;
        private readonly ILogger<IUserService> _logger;
        public UserService(IUserRepository repo, IPasswordService passwordService,
            IOptions<DatabaseSettings> options, IPersonService personService, 
            IUserRoleService userRoleService, IImageService imageService,
            IOptions<ImageSettings> imageSettings, ILogger<IUserService> logger, 
            IConfirmationService accountConfirmationService)
        {
            _repo = repo;
            _passwordService = passwordService;
            _connectionString = options.Value.DefaultConnection;
            _personService = personService;
            _userRoleService = userRoleService;
            _imageService = imageService;
            _imageSettings = imageSettings;
            _logger = logger;
            _accountConfirmationService = accountConfirmationService;
        }



        public async Task<Result<UserPublicDTO>> AddNewAsync(int personId, UserCreateDTO newUser, SqlConnection connection, SqlTransaction transaction)
        {
            var checkEmailTask =  _repo.IsExistByEmail(newUser.Email);
            var checkUsernameTask = _repo.IsExistByUsername(newUser.Username);
            await Task.WhenAll(checkEmailTask,  checkUsernameTask);

            if (!checkEmailTask.Result.IsSuccess)
                return new Result<UserPublicDTO>(false, checkEmailTask.Result.Message, null, checkEmailTask.Result.ErrorCode);
            if (checkEmailTask.Result.Data)
                //return new Result<UserPublicDTO>(false, "This email is already registered", null, 409);
                return new Result<UserPublicDTO>(false, "email_exists", null, 409);
            if (!checkUsernameTask.Result.IsSuccess)
                return new Result<UserPublicDTO>(false, checkUsernameTask.Result.Message, null, checkUsernameTask.Result.ErrorCode);
            if (checkUsernameTask.Result.Data)
                //return new Result<UserPublicDTO>(false, "This username is used by another user", null, 409);
                return new Result<UserPublicDTO>(false, "username_exists", null, 409);

            newUser.Password = _passwordService.HashPassword(newUser, newUser.Password);
            var userResult = await _repo.AddNewAsync(personId, newUser, connection, transaction);
            if (!userResult.IsSuccess)
            {
                return new Result<UserPublicDTO>(false, "internal_server_error", null, 500);
            }

            return userResult;
        }

        public async Task<Result<UserPublicDTO>> CreateAsync(UserCreateRequestDTO  userCreateRequestDTO)
        {
            var newPerson = userCreateRequestDTO.GetPersonCreateDTO();
            var newUser = userCreateRequestDTO.GetUserCreateDTO();
            GetImageUrlsResult imageUrls = null;
            if (newPerson.ProfileImage != null)
            {
                var imageUrlResult = _imageService.GetImageUrls(newPerson.ProfileImage, _imageSettings.Value.ProfileFolder);
                if (!imageUrlResult.IsSuccess)
                    return new Result<UserPublicDTO>(false, imageUrlResult.Message, null, imageUrlResult.ErrorCode);
                imageUrls = imageUrlResult.Data;
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                DbTransaction transaction = null;
                try
                {
                    await connection.OpenAsync();
                    transaction = await connection.BeginTransactionAsync();

                    var personResult = await _personService.AddNewAsync(newPerson, imageUrls.RelativeUrl,  connection,(SqlTransaction) transaction);
                    if (!personResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return new Result<UserPublicDTO>(false, personResult.Message, null, personResult.ErrorCode);
                    }
                    var userResult = await AddNewAsync(personResult.Data.Id, newUser, connection,(SqlTransaction) transaction);
                    if (!userResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return new Result<UserPublicDTO>(false, userResult.Message, null, userResult.ErrorCode);
                    }

                    var userRoleResult = await _userRoleService.AddNewAsync((int) Roles.Admin, userResult.Data.Id, true, connection, (SqlTransaction)transaction);
                    if (!userRoleResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return new Result<UserPublicDTO>(false, userRoleResult.Message, null, userRoleResult.ErrorCode);
                    }
                    userResult.Data.Roles.Add(new UserRoleInfoDTO(userRoleResult.Data.Id,Roles.Admin.ToString(), Roles.Admin.GetNameAr() , userRoleResult.Data.IsActive, userRoleResult.Data.CreatedAt, userRoleResult.Data.UpdatedAt));
                    await transaction.CommitAsync();

                    var accountConfirmationResult = await _accountConfirmationService.SendAccountConfirmationAsync(userResult.Data);

                    if (!accountConfirmationResult.IsSuccess)
                    {
                        _logger.LogWarning("Failed to send confirmation email to {Email}", userResult.Data.Email);
                    }
                    Console.WriteLine(imageUrls.PhysicalUrl);
                    await _imageService.SaveImageAsync(newPerson.ProfileImage, imageUrls.PhysicalUrl);
                    return userResult;
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                        try
                        {
                            await transaction.RollbackAsync();
                        }catch (Exception rollBackEx) 
                        {
                            _logger.LogError(rollBackEx, "failed to roll back while insert a new user");
                        }
                    _logger.LogError(ex, "failed to insert a new user");
                    return new Result<UserPublicDTO>(false, "internal_server_error", null, 500);
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

        public async Task<Result<PagedResponseDTO<UserPublicDTO>>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _repo.GetAllAsync(pageNumber, pageSize);
        }

        public async Task<Result<bool>> UpdateAsync(int id, UserUpdateDTO updatedUser)
        {
            var  currentUserResult = await _repo.GetByIdAsync(id);
            if (!currentUserResult.IsSuccess)
                return new Result<bool>(false, currentUserResult.Message, false, currentUserResult.ErrorCode);

            if (!string.Equals(currentUserResult.Data.Email, updatedUser.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existByEmailResult = await _repo.IsExistByEmail(updatedUser.Email);
                if (!existByEmailResult.IsSuccess)
                    return new Result<bool>(false, existByEmailResult.Message, false, existByEmailResult.ErrorCode);
                if (existByEmailResult.Data)
                    return new Result<bool>(false, "email_exists", false, 409);
            }
            
            if (!string.Equals(currentUserResult.Data.Username, updatedUser.Username, StringComparison.OrdinalIgnoreCase))
            {
                var existByUsernameResult = await _repo.IsExistByUsername(updatedUser.Username);
                if (!existByUsernameResult.IsSuccess)
                    return new Result<bool>(false, existByUsernameResult.Message, false, existByUsernameResult.ErrorCode);
                if (existByUsernameResult.Data)
                    return new Result<bool>(false, "username_exists", false, 409);
            }
            if (!string.IsNullOrWhiteSpace(updatedUser.Password))
            {
                updatedUser.Password = _passwordService.HashPassword(updatedUser, updatedUser.Password);
            }
            else
            {
                updatedUser.Password = currentUserResult.Data.Password; 
            }

            return await _repo.UpdateAsync(id, updatedUser);
        }


        public async Task<Result<bool>> ResetPasswordAsync(int id, string newPassword, SqlConnection conn, SqlTransaction transaction)
        {
            Result<UserDTO> userResult = await _repo.GetByIdAsync(id);
            if (!userResult.IsSuccess)
            {
                return new Result<bool>(false, userResult.Message, false, userResult.ErrorCode);
            }


            userResult.Data.Password = newPassword;

            return await _repo.ResetPasswordAsync(id, _passwordService.HashPassword(userResult.Data, userResult.Data.Password), conn, transaction);
        }

        public async Task<Result<bool>> MarkEmailAsConfirmed(int id)
        {
            return await _repo.MarkEmailAsConfirmed(id);
        }
    }
}
