using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.VariationOption;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.Business.Services
{
    public class VariationOptionService : IVariationOptionService
    {

        private readonly IVariationOptionRepository _variationOptionRepository;

        public VariationOptionService(IVariationOptionRepository variationOptionRepository)
        {
            _variationOptionRepository = variationOptionRepository;
        }

        public async Task<Result<VariationOptionDTO>> AddNewAsync(int variationId, VariationOptionCreateDTO variationOption, SqlConnection connection, SqlTransaction transaction)
        {
            return  await _variationOptionRepository.AddNewAsync(variationId, variationOption, connection, transaction);    
        }
    }
}
