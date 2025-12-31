using Jannara_Ecommerce.DTOs.Seller;
using Jannara_Ecommerce.DTOs.SellerOrderItem;

namespace Jannara_Ecommerce.DTOs.SellerOrder
{
    public class SellerOrderDetailsDTO
    {
        public int Id { get; set; }
        public int SellerId { get; set; }

        public byte OrderStatus { get; set; }
        public string StatusNameEn { get; set; }
        public string StatusNameAr { get; set; }

        public decimal SubTotal { get; set; }
        public decimal TaxCost { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal GrandTotal { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public SellerDetailsDTO Seller { get; set; }
        public List<SellerOrderItemDetailsDTO> SellerOrderItems { get; set; }
    }
}
