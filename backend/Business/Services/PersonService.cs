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
                Result<string> saveImageResult = await _imageService.SaveProfileImageAsync(personCreateDTO.ProfileImage);
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
            return await _repo.DeleteAsync(id);
        }

        public async Task<Result<PersonDTO>> FindAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Result<bool>> UpdateAsync(int id, PersonDTO updatedAccount)
        {
           return await _repo.UpdateAsync(id, updatedAccount);
        }

        
    }
}
