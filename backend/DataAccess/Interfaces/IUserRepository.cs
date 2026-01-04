using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.User;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        public Task<Result<UserDTO>> GetByIdAsync(int id);
        public Task<Result<UserDTO>> GetByEmailAsync(string email);
        public  Task<Result<PagedResponseDTO<UserPublicDTO>>> GetAllAsync(int pageNumber = 1, int pageSize = 20, int? currentUserId = null);
        public Task<Result<UserPublicDTO>> AddNewAsync(int personId, UserCreateDTO newUser, SqlConnection connection, SqlTransaction transaction);
        public Task<Result<UserDTO>> UpdateAsync(int id, UserUpdateDTO updatedUser);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<bool>> IsExistByEmail(string email);
        public Task<Result<bool>> IsExistByUsername(string username);
        public Task<Result<bool>> ResetPasswordAsync(ChangePasswordDTO resetPasswordDTO);
        public Task<Result<bool>> ResetPasswordAsync(int id, string newPassword, SqlConnection conn, SqlTransaction transaction);
        public Task<Result<bool>> MarkEmailAsConfirmed(int id);

    }
}
