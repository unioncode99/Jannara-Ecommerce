using Org.BouncyCastle.Utilities.Zlib;

namespace Jannara_Ecommerce.DTOs.Order
{
    public class OrderCreateDTO
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public int PaymentMethodId { get; set; }
        public int ShippingAddressId { get; set; }
        public int ShippingMethodId { get; set; }
        public bool PayNow { get; set; } = false;
        public decimal? TaxRate { get; set; }
        public string? TransactionReference { get; set; }
        public string? PaymentIntentId { get; set; }
        public decimal? GrandTotal { get; set; }
    }
}
