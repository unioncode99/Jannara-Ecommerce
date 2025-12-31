namespace Jannara_Ecommerce.DTOs.Wishlist
{
    public class WishlistItemDTO
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; }
        public string ProductNameEn { get; set; }
        public string ProductNameAr { get; set; }
        public string ProductImageUrl { get; set; }
        public decimal MinPrice { get; set; }
    }
}
