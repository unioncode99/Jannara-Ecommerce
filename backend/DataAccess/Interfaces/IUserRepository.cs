using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        public Task<Result<UserDTO>> GetByIdAsync(int id);
        public Task<Result<UserDTO>> GetByEmailAsync(string email);
        public Task<Result<IEnumerable<UserPublicDTO>>> GetAllAsync(int? currentUserId, int pageNumber = 1, int pageSize = 20);
        public Task<Result<UserPublicDTO>> AddNewAsync(UserDTO newUser, SqlConnection connection, SqlTransaction transaction);
        public Task<Result<bool>> UpdateAsync(int id, UserDTO updatedUser);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
