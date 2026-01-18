using Jannara_Ecommerce.DTOs.ProductItemImage;
using Jannara_Ecommerce.DTOs.SellerProductImage;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface ISellerProductImageService
    {
        Task<Result<SellerProductImageDTO>> AddNewAsync(
SellerProductImageCreateDBDTO productImage,
SqlConnection connection, SqlTransaction transaction);

        Task<Result<bool>> DeleteAsync(int id);

        Task<Result<IEnumerable<SellerProductImageDTO>>> AddNewImagesAsync(SellerProductImageCreateOneDTO productImage);

    }
}
