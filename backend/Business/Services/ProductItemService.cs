using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.ProductItem;
using Jannara_Ecommerce.DTOs.Variation;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Services
{
    public class ProductItemService : IProductItemService
    {
        private readonly IProductItemRepository _productItemRepository;

        public ProductItemService(IProductItemRepository productItemRepository)
        {
            _productItemRepository = productItemRepository;
        }

        public async Task<Result<ProductItemDTO>> AddNewAsync(int productId, string sku, SqlConnection connection, SqlTransaction transaction)
        {
            return await _productItemRepository.AddNewAsync(productId, sku, connection, transaction);   
        }
    }
}
