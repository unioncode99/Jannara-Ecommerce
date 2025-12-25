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
        public int LineCount { get; set; }  
        public decimal SubTotal { get; set; }
        public decimal TaxPrice { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal GrandTotal { get; set; }
        public IEnumerable<CartItemResponseDTO> CartItems { get; set; }
    }
}
