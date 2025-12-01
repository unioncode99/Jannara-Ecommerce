using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs
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
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "BusinessName is required.")]
        public string BusinessName { get; set; }
        public string? WebsiteUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
