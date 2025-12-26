using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.ShippingMethod;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Services
{
    public class ShippingMethodService : IShippingMethodService
    {
        private readonly IShippingMethodRepository _shippingMethodRepository;
        public ShippingMethodService(IShippingMethodRepository shippingMethodRepository)
        {
            _shippingMethodRepository = shippingMethodRepository;
        }

        public async Task<Result<IEnumerable<ShippingMethodDTO>>> GetAllActiveAsync()
        {
            return await _shippingMethodRepository.GetAllActiveAsync();
        }
    }
}
