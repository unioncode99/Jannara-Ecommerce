using Jannara_Ecommerce.DTOs.SellerProductImage;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface ISellerProductImageRepository
    {
        Task<Result<SellerProductImageDTO>> AddNewAsync(
    SellerProductImageCreateDBDTO productImage,
    SqlConnection connection, SqlTransaction transaction);

        Task<Result<bool>> DeleteAsync(int id);
    }
}
