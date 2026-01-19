using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.ProductRating;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IProductRatingService
    {
        Task<Result<ProductRatingDTO>> AddNewAsync(ProductRatingCreateDTO newRating);
        Task<Result<PagedResponseDTO<ProductRatingDTO>>> GetAllAsync(ProductRatingFilterDTO filter);
        Task<Result<ProductRatingDTO>> UpdateAsync(int id, ProductRatingUpdateDTO updatedRating);
        Task<Result<bool>> DeleteAsync(int id);
    }
}
