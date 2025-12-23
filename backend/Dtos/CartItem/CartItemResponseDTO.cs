namespace Jannara_Ecommerce.DTOs.CartItem
{
    public class CartItemResponseDTO
    {
        public int Id { get; set; }
        public int SellerProductId { get; set; }
        public byte Quantity { get; set; }
        public decimal PriceAtAddTime { get; set; }
        public decimal SubTotal { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
