using Jannara_Ecommerce.DTOs.Brand;
using Jannara_Ecommerce.DTOs.ProductCategory;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IBrandRepository
    {
        Task<Result<IEnumerable<BrandDTO>>> GetAllAsync();
        Task<Result<BrandDTO>> AddNewAsync(BrandCreateDTO newPBrand);
        Task<Result<BrandDTO>> UpdateAsync(int id, BrandUpdateDTO updatedBrand);
        Task<Result<bool>> DeleteAsync(int id);
    }
}
