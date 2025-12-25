namespace Jannara_Ecommerce.DTOs.CustomerWishlist
{
    public class CustomerWishlistDTO
    {
        public CustomerWishlistDTO(int id, int customerId, int productId, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            CustomerId = customerId;
            ProductId = productId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
