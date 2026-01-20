namespace Jannara_Ecommerce.DTOs.CustomerWishlist
{
    public class CustomerWishlistCreateDTO
    {
        public int ProductId { get; set; }
        public int? CurrentUserId { get; set; }
    }
}
