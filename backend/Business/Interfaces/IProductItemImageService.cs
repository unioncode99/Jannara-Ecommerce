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
    }
}
