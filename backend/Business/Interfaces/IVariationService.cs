using Jannara_Ecommerce.DTOs.Variation;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IVariationService
    {
        Task<Result<VariationDTO>> AddNewAsync(int productId,
            VariationCreateDTO variation,
            SqlConnection connection, SqlTransaction transaction);
        Task<Result<VariationDTO>> AddNewAsync(VariationCreateOneDTO variationCreateOneDTO);
        Task<Result<VariationDTO>> UpdateAsync(int id, VariationUpdateDTO variationUpdateDTO);
         Task<Result<bool>> DeleteAsync(int id);
    }
}
