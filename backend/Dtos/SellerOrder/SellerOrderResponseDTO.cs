using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.SellerOrderItem;

namespace Jannara_Ecommerce.DTOs.SellerOrder
{
    public class SellerOrderResponseDTO
    {
        public int Id { get; set; }
        public string PublicId { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime CreatedAt { get; set; }
        public int OrderStatus { get; set; }
        public string StatusNameEn { get; set; }
        public string StatusNameAr { get; set; }

        public IEnumerable<SellerOrderItemResponseDTO> SellerOrderItems { get; set; }
        public CustomerResponseDTO Customer { get; set; }
    }
}
