using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IUserService
    {
        public Task<Result<UserDTO>> FindAsync(int id);
        public Task<Result<UserDTO>> FindAsync(string email);
        public Task<Result<UserPublicDTO>> AddNewAsync(UserDTO newUser, SqlConnection connection, SqlTransaction transaction);
        public Task<Result<bool>> UpdateAsync(int id, UserDTO updatedUser);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
