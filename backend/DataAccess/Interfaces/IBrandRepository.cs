using Jannara_Ecommerce.DTOs.Brand;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IBrandRepository
    {
        Task<Result<IEnumerable<BrandDTO>>> GetAllAsync();
    }
}
