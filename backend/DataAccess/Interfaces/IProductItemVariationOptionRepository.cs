using Jannara_Ecommerce.DTOs.ProductItemVariationOption;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IProductItemVariationOptionRepository
    {
        Task<Result<ProductItemVariationOptionDTO>> AddNewAsync(int productItemId,
            int variationOptionId,
            SqlConnection connection, SqlTransaction transaction);
    }
}
