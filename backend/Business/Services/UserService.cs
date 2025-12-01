using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IPasswordService _passwordService;
        public UserService(IUserRepository repo, IPasswordService passwordService)
        {
            _repo = repo;
            _passwordService = passwordService;
        }
        public async Task<Result<UserPublicDTO>> AddNewAsync(UserDTO newUser)
        {
            Result<UserDTO> findResult = await FindAsync(newUser.Email);
            if (!findResult.IsSuccess)
                return new Result<UserPublicDTO>(false, "This email is already registerd", null, 409);
            newUser.Password = _passwordService.HashPassword(newUser);
            return await _repo.AddNewAsync(newUser);
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

        public Task<Result<bool>> UpdateAsync(int id, UserDTO updatedUser)
        {
            return _repo.UpdateAsync(id, updatedUser);
        }
    }
}
