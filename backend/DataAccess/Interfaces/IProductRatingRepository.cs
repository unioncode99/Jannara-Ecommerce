using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.ProductRating;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IProductRatingRepository
    {
        Task<Result<ProductRatingDTO>> AddNewAsync(ProductRatingCreateDTO newRating);
        Task<Result<PagedResponseDTO<ProductRatingDetailsDTO>>> GetAllAsync(ProductRatingFilterDTO filter);
        Task<Result<ProductRatingDTO>> UpdateAsync(int id, ProductRatingUpdateDTO updatedRating);
        Task<Result<bool>> DeleteAsync(int id);
    }
}
