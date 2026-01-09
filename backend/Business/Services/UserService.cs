using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.DTOs.Role;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.DTOs.UserRole;
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
        private readonly string _baseUrl;
        public UserService(IUserRepository repo, IPasswordService passwordService,
            IOptions<DatabaseSettings> options, IPersonService personService, 
            IUserRoleService userRoleService, IImageService imageService,
            IOptions<ImageSettings> imageSettings, ILogger<IUserService> logger, 
            IConfirmationService accountConfirmationService, IOptions<AppSettings> appSettings)
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
            _baseUrl = appSettings.Value.BaseUrl;
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

            string? profileImageUrl = null;

            // Save Image
            if (newPerson.ProfileImage != null)
            {
                var imageSaveResult = await _imageService.SaveImageAsync(
            newPerson.ProfileImage,
            _imageSettings.Value.ProfileFolder);

                if (!imageSaveResult.IsSuccess)
                    return new Result<UserPublicDTO>(
                        false,
                        imageSaveResult.Message,
                        null,
                        imageSaveResult.ErrorCode);

                profileImageUrl = imageSaveResult.Data;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                DbTransaction transaction = null;
                try
                {
                    await connection.OpenAsync();
                    transaction = await connection.BeginTransactionAsync();
                    // Create Person
                    var personResult = await _personService.AddNewAsync(newPerson, profileImageUrl,  connection,(SqlTransaction) transaction);
                    if (!personResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        if (profileImageUrl != null)
                        {
                            _imageService.DeleteImage(profileImageUrl);
                        }
                        return new Result<UserPublicDTO>(false, personResult.Message, null, personResult.ErrorCode);
                    }
                    // Create User
                    var userResult = await AddNewAsync(personResult.Data.Id, newUser, connection,(SqlTransaction) transaction);
                    if (!userResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        if (profileImageUrl != null)
                        {
                            _imageService.DeleteImage(profileImageUrl);
                        }
                        return new Result<UserPublicDTO>(false, userResult.Message, null, userResult.ErrorCode);
                    }
                    // Assign Role
                    var userRoleResult = await _userRoleService.AddNewAsync((int) Roles.Admin, userResult.Data.Id, true, connection, (SqlTransaction)transaction);
                    if (!userRoleResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        if (profileImageUrl != null)
                        {
                            _imageService.DeleteImage(profileImageUrl);
                        }
                        return new Result<UserPublicDTO>(false, userRoleResult.Message, null, userRoleResult.ErrorCode);
                    }

                    var markEmailAsConfirmedTask = await MarkEmailAsConfirmed(userResult.Data.Id, connection, (SqlTransaction)transaction);
                    if (!markEmailAsConfirmedTask.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        if (profileImageUrl != null)
                        {
                            _imageService.DeleteImage(profileImageUrl);
                        }
                        return new Result<UserPublicDTO>(false, markEmailAsConfirmedTask.Message, null, markEmailAsConfirmedTask.ErrorCode);
                    }

                    userResult.Data.Roles.Add(new UserRoleInfoDTO(userRoleResult.Data.Id, Roles.Admin.ToString(), Roles.Admin.GetNameAr(), userRoleResult.Data.IsActive, userRoleResult.Data.CreatedAt, userRoleResult.Data.UpdatedAt));
                    await transaction.CommitAsync();



                    return userResult;
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        try
                        {
                            await transaction.RollbackAsync();
                        }
                        catch (Exception rollBackEx)
                        {
                            _logger.LogError(rollBackEx, "failed to roll back while insert a new user");
                        }
                    }

                    if (profileImageUrl != null)
                    {
                        _imageService.DeleteImage(profileImageUrl);
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

        public async Task<Result<PagedResponseDTO<UserDetailsDTO>>> GetAllAsync(FilterUserDTO filterUserDTO)
        {
            var usersResult = await _repo.GetAllAsync(filterUserDTO);
            if (usersResult.IsSuccess)
            {
                foreach (var user in usersResult.Data.Items)
                {
                   user.Person.ImageUrl = ImageUrlHelper.ToAbsoluteUrl(user.Person.ImageUrl, _baseUrl);
                }
            }
            return usersResult;
        }

        public async Task<Result<UserPublicDTO>> UpdateAsync(int id, UserUpdateDTO updatedUser)
        {
            // Get current user
            var  currentUserResult = await _repo.GetByIdAsync(id);
            if (!currentUserResult.IsSuccess)
            {
                return new Result<UserPublicDTO>(false, currentUserResult.Message, null, currentUserResult.ErrorCode);
            }
            var currentUser = currentUserResult.Data;
            // Check username
            if (!string.Equals(currentUser.Username, updatedUser.Username, StringComparison.OrdinalIgnoreCase))
            {
                var existByUsernameResult = await _repo.IsExistByUsername(updatedUser.Username);
                if (!existByUsernameResult.IsSuccess)
                {
                    return new Result<UserPublicDTO>(false, existByUsernameResult.Message, null, existByUsernameResult.ErrorCode);
                }
                if (existByUsernameResult.Data)
                {
                    return new Result<UserPublicDTO>(false, "username_exists", null, 409);
                }
            }
            // Handle password
            if (!string.IsNullOrWhiteSpace(updatedUser.Password))
            {
                updatedUser.Password = _passwordService.HashPassword(updatedUser, updatedUser.Password);
            }
            else
            {
                updatedUser.Password = currentUser.Password; 
            }

            var updateResult = await _repo.UpdateAsync(id, updatedUser);
            return new Result<UserPublicDTO>(true, updateResult.Message, updateResult.Data.ToUserPublicDTO(), updateResult.ErrorCode);
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

        public async Task<Result<bool>> ResetPasswordAsync(ChangePasswordDTO resetPasswordDTO)
        {

            var userResult = await FindAsync((int)resetPasswordDTO.UserId);

            if (!userResult.IsSuccess)
            {
                return new Result<bool>(false, "User not found", false, 404);
            }
            var isValidOldPassword = _passwordService.VerifyPassword<UserDTO>(null, userResult.Data.Password, resetPasswordDTO.OldPassword);

            if (!isValidOldPassword)
            {
                return new Result<bool>(false, "Old password is incorrect", false, 400);
            }

            resetPasswordDTO.NewPassword =  _passwordService.HashPassword<UserDTO>(null, resetPasswordDTO.NewPassword);
            resetPasswordDTO.OldPassword =  _passwordService.HashPassword<UserDTO>(null, resetPasswordDTO.OldPassword);
            return await _repo.ResetPasswordAsync(resetPasswordDTO);
        }

        public async Task<Result<bool>> MarkEmailAsConfirmed(int id, SqlConnection conn, SqlTransaction transaction)
        {
            return await _repo.MarkEmailAsConfirmed(id, conn, transaction);
        }

    }
}
