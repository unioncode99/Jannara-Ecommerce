using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.Enums;
using Jannara_Ecommerce.Mappers;
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
        private readonly IOptions<ImageSettings> _imageSettings;
        private readonly IImageService _imageService;
        private readonly ILogger<ISellerService> _logger;
        public SellerService(ISellerRepository repo, IPersonService PersonService, 
            IUserService UserService, IOptions<DatabaseSettings> options,
            IUserRoleService userRoleService ,IOptions<ImageSettings> imageSettings, IImageService imageService,
            ILogger<ISellerService> logger)
        {
            _repo  = repo;
            _connectionString = options.Value.DefaultConnection;
            _personService = PersonService;
            _userService = UserService;
            _userRoleService = userRoleService;
            _imageSettings = imageSettings;
            _imageService = imageService;
            _logger = logger;
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

            string imageUrl = null;
            if (newPerson.ProfileImage != null)
            {
                var imageUrlResult = _imageService.GetImageUrl(newPerson.ProfileImage, _imageSettings.Value.ProfileFolder);
                if (!imageUrlResult.IsSuccess)
                    return new Result<SellerDTO>(false, imageUrlResult.Message, null, imageUrlResult.ErrorCode);
                imageUrl = imageUrlResult.Data;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                DbTransaction transaction = null;
                try
                {
                    await connection.OpenAsync();
                    transaction = await connection.BeginTransactionAsync();
                    var personResult = await _personService.AddNewAsync(newPerson, imageUrl?.Split("wwwroot/")[1], connection,(SqlTransaction) transaction);
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
                    if (sellerCreateRequestDTO.ProfileImage != null)
                        await _imageService.SaveImageAsync(newPerson.ProfileImage, imageUrl);
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
                        catch(Exception rollBackEx) 
                        {
                            _logger.LogError(rollBackEx, "Failed to rollback while create a new seller");
                        }
                    }
                    _logger.LogError(ex, "Failed to insert a new seller");
                    return new Result<SellerDTO>(false, "internal_server_error", null, 500);
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
