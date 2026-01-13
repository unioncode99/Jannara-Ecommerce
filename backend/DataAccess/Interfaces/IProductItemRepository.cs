using Jannara_Ecommerce.DTOs.ProductItem;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IProductItemRepository
    {
        Task<Result<ProductItemDTO>> AddNewAsync(int productId,
            string sku,
            SqlConnection connection, SqlTransaction transaction);
    }
}
