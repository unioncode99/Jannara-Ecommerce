using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.Utilities;
using Jannara_Ecommerce.Utilities.WrapperClasses;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.Business.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repo;
        private readonly IImageService _imageService;
        private readonly IOptions<ImageSettings> _imageOptions;
        private readonly string _baseUrl;
        public PersonService(IPersonRepository repo, IImageService imageService, 
            IOptions<ImageSettings> imageSettings, IOptions<AppSettings> appSettings)
        {
            _repo = repo;
            _imageService = imageService;
            _imageOptions = imageSettings;
            _baseUrl = appSettings.Value.BaseUrl;
        }
        public async Task<Result<PersonDTO>> AddNewAsync(PersonCreateDTO personCreateDTO, string imageUrl, SqlConnection connection, SqlTransaction transaction)
        {
            var personResult = await _repo.AddNewAsync(personCreateDTO, imageUrl, connection, transaction);
            personResult.Data.ImageUrl = ImageUrlHelper.ToAbsoluteUrl(personResult.Data.ImageUrl, _baseUrl);
            return personResult;
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var findResult = await FindAsync(id);
            if (!findResult.IsSuccess) 
                return new Result<bool>(false, findResult.Message, false, findResult.ErrorCode);
            var deleteImageResult =  _imageService.DeleteImage(findResult.Data.ImageUrl);
            if (!deleteImageResult.IsSuccess)
                return new Result<bool>(false, deleteImageResult.Message, false, deleteImageResult.ErrorCode);
            return await _repo.DeleteAsync(id);
        }

        public async Task<Result<PersonDTO>> FindAsync(int id)
        {
            var personResult = await _repo.GetByIdAsync(id);
            personResult.Data.ImageUrl = ImageUrlHelper.ToAbsoluteUrl(personResult.Data.ImageUrl, _baseUrl);
            return personResult;
        }

        public async Task<Result<PersonDTO>> UpdateAsync(int id, PersonUpdateDTO updatedPerson)
        {
            // Fetch existing person
            var findResult = await _repo.GetByIdAsync(id);
            if (!findResult.IsSuccess)
            {
                return new Result<PersonDTO>(false, findResult.Message, null, findResult.ErrorCode);
            }

            string? finalImageUrl = findResult.Data.ImageUrl;
            // Handle delete-only request
            if (updatedPerson.DeleteProfileImage && !string.IsNullOrWhiteSpace(finalImageUrl))
            {
                var deleteResult = _imageService.DeleteImage(finalImageUrl);
                if (!deleteResult.IsSuccess)
                {
                    return new Result<PersonDTO>(false, deleteResult.Message, null, deleteResult.ErrorCode);
                }

                finalImageUrl = null;
            }
            // Handle new uploaded image
            if (updatedPerson.ProfileImage != null)
            {
                // Delete old image if it exists (after deleteProfileImage handled)
                if (!string.IsNullOrWhiteSpace(finalImageUrl))
                {
                    var deleteOld = _imageService.DeleteImage(finalImageUrl);
                    if (!deleteOld.IsSuccess)
                    {
                        return new Result<PersonDTO>(false, deleteOld.Message, null, deleteOld.ErrorCode);
                    }
                }
                // Save new image
                var saveResult = await _imageService.SaveImageAsync(
           updatedPerson.ProfileImage,
           _imageOptions.Value.ProfileFolder);

                if (!saveResult.IsSuccess)
                {
                    return new Result<PersonDTO>(false, saveResult.Message, null, saveResult.ErrorCode);
                }
                // Update final image path for DB
                finalImageUrl = saveResult.Data;
            }
            // Update DB with new image path
            var updateResult = await _repo.UpdateAsync(id, updatedPerson, finalImageUrl);
            updateResult.Data.ImageUrl = ImageUrlHelper.ToAbsoluteUrl(updateResult.Data.ImageUrl, _baseUrl);
            return updateResult;
        }



    }
}
