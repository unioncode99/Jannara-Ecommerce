using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.ProductItemImage;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Services
{
    public class ProductItemImageService : IProductItemImageService
    {

        private readonly IProductItemImageRepository _productItemImageRepository;

        public ProductItemImageService(IProductItemImageRepository productItemImageRepository)
        {
            _productItemImageRepository = productItemImageRepository;
        }

        public async Task<Result<ProductItemImageDTO>> AddNewAsync(int productItemId, ProductItemImageCreateDBDTO productItemImage, SqlConnection connection, SqlTransaction transaction)
        {
            return await _productItemImageRepository.AddNewAsync(productItemId, productItemImage, connection, transaction);
        }
    }
}
