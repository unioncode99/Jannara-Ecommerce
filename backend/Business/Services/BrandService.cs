using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Brand;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<Result<BrandDTO>> AddNewAsync(BrandCreateDTO newPBrand)
        {
            return await _brandRepository.AddNewAsync(newPBrand);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _brandRepository.DeleteAsync(id);
        }

        public async Task<Result<IEnumerable<BrandDTO>>> GetAllAsync()
        {
            return await _brandRepository.GetAllAsync();
        }

        public async Task<Result<BrandDTO>> UpdateAsync(int id, BrandUpdateDTO updatedBrand)
        {
            return await _brandRepository.UpdateAsync(id, updatedBrand);
        }
    }
}
