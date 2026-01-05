using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.ProductCategory;
using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IProductCategoryService
    {
        public Task<Result<IEnumerable<ProductCategoryDTO>>> GetAllAsync();
        public Task<Result<ProductCategoryDTO>> AddNewAsync(ProductCategoryCreateDTO newProductCategory);
        public Task<Result<ProductCategoryDTO>> UpdateAsync(int id, ProductCategoryUpdateDTO updateProductCategory);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
