namespace Jannara_Ecommerce.DTOs.Seller
{
    public class SellerDetailsDTO
    {
        public long Id { get; set; }
        public string BusinessName { get; set; }
        public string WebsiteUrl { get; set; }

        public long UserId { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string ImageUrl { get; set; }
    }
}
