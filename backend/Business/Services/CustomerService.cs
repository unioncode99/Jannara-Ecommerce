using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.Enums;
using Jannara_Ecommerce.Mappers;
using Jannara_Ecommerce.Utilities;
using Jannara_Ecommerce.Utilities.WrapperClasses;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data.Common;

namespace Jannara_Ecommerce.Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;
        private readonly string _connectionString;
        private readonly IPersonService _personService;
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IConfirmationService _confirmationService;
        private readonly IImageService _imageService;
        private readonly IOptions<ImageSettings> _imageSettings;
        private readonly ILogger<ICustomerService> _logger;
        public CustomerService(ICustomerRepository repo, IPersonService PersonService,
            IUserService UserService, IOptions<DatabaseSettings> dateBaseSettings,
            IUserRoleService userRoleService, IImageService imageService,
            IOptions<ImageSettings> imageSettings, ILogger<ICustomerService> logger, 
            IConfirmationService confirmationService)
        {
            _repo = repo;
            _connectionString = dateBaseSettings.Value.DefaultConnection;
            _personService = PersonService;
            _userService = UserService;
            _userRoleService = userRoleService;
            _imageService = imageService;
            _imageSettings = imageSettings;
            _logger = logger;
            _confirmationService = confirmationService;
        }
        public async Task<Result<CustomerDTO>> AddNewAsync(int userId, SqlConnection connection, SqlTransaction transaction)
        {
            return await _repo.AddNewAsync(userId, connection, transaction);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<Result<CustomerDTO>> FindAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }
        public async Task<Result<CustomerDTO>> CreateAsync(CustomerCreateRequestDTO customerCreateRequestDTO)
        {
          

            var personCreateDTO = customerCreateRequestDTO.GetPersonCreateDTO();
            var userCreateDTO = customerCreateRequestDTO.GetUserCreateDTO();

          
                GetImageUrlsResult imageUrls = null;
            if (personCreateDTO.ProfileImage != null)
            {
                var imageUrlResult = _imageService.GetImageUrls(personCreateDTO.ProfileImage, _imageSettings.Value.ProfileFolder);
                if (!imageUrlResult.IsSuccess) 
                    return new Result<CustomerDTO>(false, imageUrlResult.Message, null, imageUrlResult.ErrorCode);
                imageUrls = imageUrlResult.Data;
            }
               
                

            using (var connection = new SqlConnection(_connectionString))
            {
                DbTransaction transaction = null;
                try
                {
                    await connection.OpenAsync();
                    transaction = await connection.BeginTransactionAsync();
                    var personResult = await _personService.AddNewAsync(personCreateDTO, imageUrls.RelativeUrl, connection,(SqlTransaction) transaction);
                    if (!personResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return new Result<CustomerDTO>(false, personResult.Message, null, personResult.ErrorCode);
                    }
                    
                    var userResult = await _userService.AddNewAsync(personResult.Data.Id, userCreateDTO, connection, (SqlTransaction)transaction);
                    if (!userResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return new Result<CustomerDTO>(false, userResult.Message, null, userResult.ErrorCode);
                    }

                    var userRoleResult = await _userRoleService.AddNewAsync((int)Roles.Customer, userResult.Data.Id, true, connection, (SqlTransaction)transaction);
                    if (!userRoleResult.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return new Result<CustomerDTO>(false, userRoleResult.Message, null, userRoleResult.ErrorCode);
                    }

                    var customerResult = await AddNewAsync(userResult.Data.Id,  connection, (SqlTransaction)transaction);
                    if (!customerResult.IsSuccess)
                    {
                       await transaction.RollbackAsync();
                        return new Result<CustomerDTO>(false, customerResult.Message, null, customerResult.ErrorCode);
                    }
                    await transaction.CommitAsync();

                    var accountConfirmationResult = await _confirmationService.SendAccountConfirmationAsync(userResult.Data);

                    if (!accountConfirmationResult.IsSuccess)
                    {
                        _logger.LogWarning("Failed to send confirmation email to {Email}", userResult.Data.Email);
                    }

                    if (personCreateDTO.ProfileImage != null)
                    {
                        await _imageService.SaveImageAsync(personCreateDTO.ProfileImage, imageUrls.PhysicalUrl);
                    }
                    return customerResult;
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
                            _logger.LogError(rollBackEx, "Roll back failed while create a new customer");
                        }
                    }
                    _logger.LogError(ex,"Failed to create a new customer");
                    return new Result<CustomerDTO>(false, "internal_server_error", null, 500);
                }
            }
        }

        public async Task<Result<bool>> UpdateAsync(int id, CustomerDTO updatedCustomer)
        {
            return await _repo.UpdateAsync(id, updatedCustomer);
        }

        public Task<Result<PagedResponseDTO<CustomerDTO>>> GetAllAsync(int pageNumber, int pageSize)
        {
            return _repo.GetAllAsync(pageNumber, pageSize);
        }
    }
}
