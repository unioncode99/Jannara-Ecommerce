namespace Jannara_Ecommerce.Dtos
{
    public class SellerDTO
    {
        public SellerDTO(int id, int userId, string businessName, string? websiteUrl, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            UserId = userId;
            BusinessName = businessName;
            WebsiteUrl = websiteUrl;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string BusinessName { get; set; }
        public string? WebsiteUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
