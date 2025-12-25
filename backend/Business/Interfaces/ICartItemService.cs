using Jannara_Ecommerce.DTOs.CartItem;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface ICartItemService
    {
        Task<Result<CartItemDTO>> AddOrUpdateAsync(CartItemRequestDTO cartItemRequest);
        Task<Result<bool>> DeleteAsync(int id);
    }
}
