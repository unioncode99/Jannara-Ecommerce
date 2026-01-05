using Jannara_Ecommerce.DTOs.ProductCategory;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IProductCategoryRepository
    {
        public Task<Result<IEnumerable<ProductCategoryDTO>>> GetAllAsync();
        public Task<Result<ProductCategoryDTO>> AddNewAsync(ProductCategoryCreateDTO newProductCategory);
        public Task<Result<ProductCategoryDTO>> UpdateAsync(int id, ProductCategoryUpdateDTO updateProductCategory);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
