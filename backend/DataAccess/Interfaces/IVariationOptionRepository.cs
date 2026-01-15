using Jannara_Ecommerce.DTOs.Variation;
using Jannara_Ecommerce.DTOs.VariationOption;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IVariationOptionRepository
    {
        Task<Result<VariationOptionDTO>> AddNewAsync(int variationId,
            VariationOptionCreateDTO variationOption,
            SqlConnection connection, SqlTransaction transaction);


        Task<Result<VariationOptionDTO>> AddNewAsync(VariationOptionCreateOneDTO variationOptionCreateOneDTO);
        Task<Result<VariationOptionDTO>> UpdateAsync(int id, VariationOptionUpdateDTO variationOptionUpdateDTO);
        Task<Result<bool>> DeleteAsync(int id);
    }
}
