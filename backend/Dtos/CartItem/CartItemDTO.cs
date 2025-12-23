namespace Jannara_Ecommerce.DTOs.CartItem
{
    public class CartItemDTO
    {
        public CartItemDTO(int id, int cartId, int sellerProductId, byte quantity, decimal priceAtAddTime, decimal subTotal, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            CartId = cartId;
            SellerProductId = sellerProductId;
            Quantity = quantity;
            PriceAtAddTime = priceAtAddTime;
            SubTotal = subTotal;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public int CartId { get; set; }
        public int SellerProductId { get; set; }
        public byte Quantity { get; set; }
        public decimal PriceAtAddTime { get; set; }
        public decimal SubTotal { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
