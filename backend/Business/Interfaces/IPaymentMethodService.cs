using Jannara_Ecommerce.DTOs.PaymentMethod;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IPaymentMethodService
    {
        public Task<Result<IEnumerable<PaymentMethodDTO>>> GetAllActiveAsync();
    }
}
