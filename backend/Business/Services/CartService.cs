using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DTOs.Cart;
using Jannara_Ecommerce.Utilities;

namespace Jannara_Ecommerce.Business.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Result<bool>> ClearCartAsync(int cartId)
        {
            return await _cartRepository.ClearCartAsync(cartId);
        }

        public async Task<Result<CartResponseDTO>> GetActiveCartAsync(int customerId)
        {
            return await _cartRepository.GetActiveCartAsync(customerId);
        }
    }
}
