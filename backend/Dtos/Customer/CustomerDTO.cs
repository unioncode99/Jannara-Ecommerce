using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs.Customer
{
    public class CustomerDTO
    {
        public CustomerDTO(int id, int userId, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            UserId = userId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
