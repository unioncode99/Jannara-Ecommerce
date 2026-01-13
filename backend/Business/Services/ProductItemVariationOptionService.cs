using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.ProductItemVariationOption;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Services
{
    public class ProductItemVariationOptionService : IProductItemVariationOptionService
    {
        private readonly IProductItemVariationOptionRepository _productItemVariationOption;

        public ProductItemVariationOptionService(IProductItemVariationOptionRepository productItemVariationOptionRepository)
        {
            _productItemVariationOption = productItemVariationOptionRepository;
        }

        public async Task<Result<ProductItemVariationOptionDTO>> AddNewAsync(int productItemId, int variationOptionId, SqlConnection connection, SqlTransaction transaction)
        {
            return await _productItemVariationOption.AddNewAsync(productItemId, variationOptionId, connection, transaction);    
        }
    }
}
