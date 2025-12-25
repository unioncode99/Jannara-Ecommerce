using Jannara_Ecommerce.DTOs.CartItem;
using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.Utilities;
using Microsoft.Data.SqlClient;

namespace Jannara_Ecommerce.DataAccess.Interfaces
{
    public interface ICartItemRepository
    {
        Task<Result<CartItemDTO>> AddOrUpdateAsync(CartItemRequestDTO cartItemRequest);
        Task<Result<bool>> DeleteAsync(int id);
    }
}
