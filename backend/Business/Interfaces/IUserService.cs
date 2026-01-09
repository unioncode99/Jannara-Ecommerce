using Jannara_Ecommerce.Business.Services;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Role;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IUserService
    {
        public Task<Result<UserDTO>> FindAsync(int id);
        public Task<Result<UserDTO>> FindAsync(string email);
        public Task<Result<UserPublicDTO>> AddNewAsync(int personId, UserCreateDTO newUser, SqlConnection connection, SqlTransaction transaction);
        public Task<Result<UserPublicDTO>> CreateAsync(UserCreateRequestDTO userCreateRequestDTO);
        public Task<Result<UserPublicDTO>> UpdateAsync(int id, UserUpdateDTO updatedUser);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<PagedResponseDTO<UserDetailsDTO>>> GetAllAsync(FilterUserDTO filterUserDTO);
        public Task<Result<bool>> ResetPasswordAsync(int id, string newPassword, SqlConnection conn, SqlTransaction transaction);
        public Task<Result<bool>> ResetPasswordAsync(ChangePasswordDTO resetPasswordDTO);
        public Task<Result<bool>> MarkEmailAsConfirmed(int id);
        public Task<Result<bool>> MarkEmailAsConfirmed(int id, SqlConnection conn, SqlTransaction transaction);

    }
}
