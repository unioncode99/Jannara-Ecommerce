using Jannara_Ecommerce.DTOs.Role;
using Jannara_Ecommerce.DTOs.Variation;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IVariationRepository
    {
        Task<Result<VariationDTO>> AddNewAsync(int productId,
            VariationCreateDTO variation,
            SqlConnection connection, SqlTransaction transaction);

        public Task<Result<VariationDTO>> AddNewAsync(VariationCreateOneDTO variationCreateOneDTO);
        public Task<Result<VariationDTO>> UpdateAsync(int id, VariationUpdateDTO variationUpdateDTO);
        public Task<Result<bool>> DeleteAsync(int id);
    }
}
