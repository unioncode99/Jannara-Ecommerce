using Jannara_Ecommerce.DTOs.CartItem;

namespace Jannara_Ecommerce.DTOs.Cart
{
    public class CartResponseDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ItemsCount { get; set; }  
        public decimal TotalPrice { get; set; }
        public IEnumerable<CartItemResponseDTO> CartItems { get; set; }
    }
}
