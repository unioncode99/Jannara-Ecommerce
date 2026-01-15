using Jannara_Ecommerce.DTOs.ProductItemImage;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IProductItemImageService
    {
        Task<Result<ProductItemImageDTO>> AddNewAsync(int productItemId,
            ProductItemImageCreateDBDTO productItemImage,
            SqlConnection connection, SqlTransaction transaction);


        Task<Result<IEnumerable<ProductItemImageDTO>>> AddNewImagesAsync(ProductItemImageCreateOneDTO productItem);
        public Task<Result<ProductItemImageDTO>> SetPrimaryAsync(int id);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
