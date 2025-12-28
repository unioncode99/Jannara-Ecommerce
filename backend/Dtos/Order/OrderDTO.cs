using System.Runtime.Intrinsics.X86;

namespace Jannara_Ecommerce.DTOs.Order
{
    public class OrderDTO
    {
        public OrderDTO(int id, string publicOrderId, int customerId, int shippingAddressId, int shippingMethodId, string paymentIntentId, byte orderStatus, decimal subtotal, decimal taxCost, decimal shippingCost, decimal grandTotal, DateTime placedAt, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            PublicOrderId = publicOrderId;
            CustomerId = customerId;
            ShippingAddressId = shippingAddressId;
            ShippingMethodId = shippingMethodId;
            PaymentIntentId = paymentIntentId;
            OrderStatus = orderStatus;
            Subtotal = subtotal;
            TaxCost = taxCost;
            ShippingCost = shippingCost;
            GrandTotal = grandTotal;
            PlacedAt = placedAt;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public string PublicOrderId { get; set; }
        public int CustomerId { get; set; }
        public int ShippingAddressId { get; set; }
        public int ShippingMethodId { get; set; }
        public string PaymentIntentId { get; set; }
        public byte OrderStatus { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxCost { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
