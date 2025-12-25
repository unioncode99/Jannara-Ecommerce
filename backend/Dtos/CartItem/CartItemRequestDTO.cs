namespace Jannara_Ecommerce.DTOs.CartItem
{
    public class CartItemRequestDTO
    {
        public int CustomerId { get; set; }
        public int SellerProductId { get; set; }
        public byte Quantity { get; set; }
    }
}
