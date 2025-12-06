using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.Dtos.Person;
using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repo;
        private readonly IImageService _imageService;
        private readonly string _folderName = "profiles";
        public PersonService(IPersonRepository repo, IImageService imageService)
        {
            _repo = repo;
            _imageService = imageService;
        }
        public async Task<Result<PersonDTO>> AddNewAsync(PersonCreateDTO personCreateDTO, SqlConnection connection, SqlTransaction transaction)
        {
            string imageUrl = string.Empty;
            if (personCreateDTO.ProfileImage != null)
            {
                var saveImageResult = await _imageService.SaveImageAsync(personCreateDTO.ProfileImage,_folderName);
                if (!saveImageResult.IsSuccess)
                    return new Result<PersonDTO>(false, saveImageResult.Message, null, saveImageResult.ErrorCode);
                imageUrl = saveImageResult.Data;
            }
            else
                imageUrl = null;
                
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
                var saveImageResult = await _imageService.SaveImageAsync(updatedPerson.ProfileImage, _folderName);
                if (!saveImageResult.IsSuccess)
                    return new Result<bool>(false, saveImageResult.Message, false, saveImageResult.ErrorCode);
                imageUrl = saveImageResult.Data;
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

            return await _repo.UpdateAsync(id, updatedPerson, imageUrl);
        }

        
    }
}
