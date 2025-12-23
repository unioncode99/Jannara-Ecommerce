using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.CartItem;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        public CartItemService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }
        public async Task<Result<CartItemDTO>> AddOrUpdateAsync(CartItemRequestDTO cartItemRequest)
        {
            return await _cartItemRepository.AddOrUpdateAsync(cartItemRequest);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            return await _cartItemRepository.DeleteAsync(id);
        }
    }
}
