using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DataAccess.Repositories;
using Jannara_Ecommerce.DTOs.General;
using Jannara_Ecommerce.DTOs.ProductRating;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Services
{
    public class ProductRatingService : IProductRatingService
    {
        private readonly IProductRatingRepository _productRatingRepository;
        public ProductRatingService(IProductRatingRepository productRatingRepository)
        {
            _productRatingRepository = productRatingRepository;
        }

        public async Task<Result<ProductRatingDTO>> AddNewAsync(ProductRatingCreateDTO newRating)
        {
            return await _productRatingRepository.AddNewAsync(newRating);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _productRatingRepository.DeleteAsync(id);
        }

        public async Task<Result<PagedResponseDTO<ProductRatingDetailsDTO>>> GetAllAsync(ProductRatingFilterDTO filter)
        {
            return await _productRatingRepository.GetAllAsync(filter);
        }

        public Task<Result<ProductRatingDTO>> UpdateAsync(int id, ProductRatingUpdateDTO updatedRating)
        {
            return _productRatingRepository.UpdateAsync(id, updatedRating);
        }
    }
}
