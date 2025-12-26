using Jannara_Ecommerce.DTOs.Address;
using Jannara_Ecommerce.DTOs.ShippingMethod;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IShippingMethodRepository
    {
        public Task<Result<IEnumerable<ShippingMethodDTO>>> GetAllActiveAsync();
    }
}
