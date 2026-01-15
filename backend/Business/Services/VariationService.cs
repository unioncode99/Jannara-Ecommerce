using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Variation;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Jannara_Ecommerce.Business.Services
{
    public class VariationService : IVariationService
    {
        private readonly IVariationRepository _variationRepository;

        public VariationService(IVariationRepository variationRepository)
        {
            _variationRepository = variationRepository;
        }

        public async Task<Result<VariationDTO>> AddNewAsync(int productId, VariationCreateDTO variation, SqlConnection connection, SqlTransaction transaction)
        {
            return await _variationRepository.AddNewAsync(productId, variation, connection, transaction);
        }

        public async Task<Result<VariationDTO>> AddNewAsync(VariationCreateOneDTO variationCreateOneDTO)
        {
            return await _variationRepository.AddNewAsync(variationCreateOneDTO);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _variationRepository.DeleteAsync(id);
        }

        public async Task<Result<VariationDTO>> UpdateAsync(int id, VariationUpdateDTO variationUpdateDTO)
        {
            return await _variationRepository.UpdateAsync(id, variationUpdateDTO);
        }
    }
}
