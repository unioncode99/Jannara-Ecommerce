using Jannara_Ecommerce.DTOs.ProductItemImage;
using Jannara_Ecommerce.DTOs.Variation;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IProductItemImageRepository
    {
        Task<Result<ProductItemImageDTO>> AddNewAsync(int productItemId,
            ProductItemImageCreateDBDTO productItemImage,
            SqlConnection connection, SqlTransaction transaction);

        public Task<Result<ProductItemImageDTO>> SetPrimaryAsync(int id);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
