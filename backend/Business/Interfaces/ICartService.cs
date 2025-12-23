using Jannara_Ecommerce.DTOs.Cart;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface ICartService
    {
        Task<Result<bool>> ClearCartAsync(int cartId);
        Task<Result<CartResponseDTO>> GetActiveCartAsync(int customerId);
    }
}
