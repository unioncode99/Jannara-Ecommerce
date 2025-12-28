using Jannara_Ecommerce.DTOs.OrderItem;

namespace Jannara_Ecommerce.DTOs.Order
{
    public class OrderDetailsDTO
    {
        public int Id { get; set; }
        public string PublicOrderId { get; set; }
        public int CustomerId { get; set; }
        public int ShippingAddressId { get; set; }
        public int ShippingMethodId { get; set; }
        public string PaymentIntentId { get; set; }
        public byte OrderStatus { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxCost { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public IEnumerable<OrderItemDetailsDTO> OrderItems { get; set; }
    }
}
