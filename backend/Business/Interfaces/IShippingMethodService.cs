using Jannara_Ecommerce.DTOs.ShippingMethod;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IShippingMethodService
    {
        public Task<Result<IEnumerable<ShippingMethodDTO>>> GetAllActiveAsync();
    }
}
