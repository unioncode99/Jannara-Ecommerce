using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Brand;
using Jannara_Ecommerce.Utilities;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IBrandService
    {
        Task<Result<IEnumerable<BrandDTO>>> GetAllAsync();
        Task<Result<BrandDTO>> AddNewAsync(BrandCreateDTO newPBrand);
        Task<Result<BrandDTO>> UpdateAsync(int id, BrandUpdateDTO updatedBrand);
        Task<Result<bool>> DeleteAsync(int id);
    }
}
