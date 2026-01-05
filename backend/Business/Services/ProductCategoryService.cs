using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.ProductCategory;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        public ProductCategoryService(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<Result<ProductCategoryDTO>> AddNewAsync(ProductCategoryCreateDTO newProductCategory)
        {
            return await _productCategoryRepository.AddNewAsync(newProductCategory);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _productCategoryRepository.DeleteAsync(id);
        }

        public async Task<Result<IEnumerable<ProductCategoryDTO>>> GetAllAsync()
        {
            return await _productCategoryRepository.GetAllAsync();
        }

        public async Task<Result<ProductCategoryDTO>> UpdateAsync(int id, ProductCategoryUpdateDTO updateProductCategory)
        {
            return await _productCategoryRepository.UpdateAsync(id, updateProductCategory);
        }
    }
}
