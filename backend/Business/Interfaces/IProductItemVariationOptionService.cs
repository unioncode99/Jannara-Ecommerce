using Jannara_Ecommerce.DTOs.ProductItemVariationOption;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IProductItemVariationOptionService
    {
        Task<Result<ProductItemVariationOptionDTO>> AddNewAsync(int productItemId,
            int variationOptionId,
            SqlConnection connection, SqlTransaction transaction);
    }
}
