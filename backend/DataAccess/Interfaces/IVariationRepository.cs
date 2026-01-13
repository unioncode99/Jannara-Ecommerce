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
    }
}
