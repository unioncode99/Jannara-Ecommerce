using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs.SellerOrder
{
    public class ChangeSellerOrderStatusRequest
    {
        public int? OrderId { get; set; }
        public string? PublicId { get; set; }
        [Range(1, 5)]
        public int OrderStatus { get; set; }
    }
}
