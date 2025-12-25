using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IProductCategoryRepository
    {
        public Task<Result<IEnumerable<ProductCategoryDTO>>> GetAllAsync();
    }
}
