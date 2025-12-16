using Jannara_Ecommerce.DTOs;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IProductCategoryService
    {
        public Task<Result<IEnumerable<ProductCategoryDTO>>> GetAllAsync();
    }
}
