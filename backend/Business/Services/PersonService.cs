using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.Business.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repo;
        private readonly IImageService _imageService;
        private readonly IOptions<ImageSettings> _imageOptions;
        public PersonService(IPersonRepository repo, IImageService imageService, IOptions<ImageSettings> imageSettings)
        {
            _repo = repo;
            _imageService = imageService;
            _imageOptions = imageSettings;
        }
        public async Task<Result<PersonDTO>> AddNewAsync(PersonCreateDTO personCreateDTO, string imageUrl, SqlConnection connection, SqlTransaction transaction)
        {  
            return await _repo.AddNewAsync(personCreateDTO, imageUrl, connection, transaction);
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
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Result<bool>> UpdateAsync(int id, PersonUpdateDTO updatedPerson)
        {
            var findResult = await _repo.GetByIdAsync(id);
            if (!findResult.IsSuccess)
                return new Result<bool>(false, findResult.Message, false, findResult.ErrorCode);

            string imageUrl = null;
            if (updatedPerson.ProfileImage != null)
            {
                var ImageUrlResult = _imageService.GetImageUrl(updatedPerson.ProfileImage, _imageOptions.Value.ProfileFolder);
                if (!ImageUrlResult.IsSuccess)
                    return new Result<bool>(false, ImageUrlResult.Message, false, ImageUrlResult.ErrorCode);
                imageUrl = ImageUrlResult.Data;
            }
            else if (updatedPerson.DeleteProfileImage && findResult.Data.ImageUrl is not null)
            {
                var deleteImageResult = _imageService.DeleteImage(findResult.Data.ImageUrl);
                if (!deleteImageResult.IsSuccess)
                    return new Result<bool>(false, deleteImageResult.Message, false, deleteImageResult.ErrorCode);
                imageUrl = null;
            }
            else
                imageUrl = findResult.Data.ImageUrl;
            if (imageUrl != null) 
                await _imageService.ReplaceImageAsync(findResult.Data.ImageUrl,imageUrl, updatedPerson.ProfileImage);
            return await _repo.UpdateAsync(id, updatedPerson, imageUrl);
        }

        
    }
}
