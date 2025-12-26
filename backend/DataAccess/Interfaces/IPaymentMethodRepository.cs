using Jannara_Ecommerce.DTOs.Address;
using Jannara_Ecommerce.DTOs.PaymentMethod;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface IPaymentMethodRepository
    {
        public Task<Result<IEnumerable<PaymentMethodDTO>>> GetAllActiveAsync();
    }
}
