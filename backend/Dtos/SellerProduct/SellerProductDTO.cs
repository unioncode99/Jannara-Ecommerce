namespace Jannara_Ecommerce.DTOs.SellerProduct
{
    public class SellerProductDTO
    {
        public SellerProductDTO(int id, int sellerId, int productItemId, decimal price, int stockQuantity, bool isActive, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            SellerId = sellerId;
            ProductItemId = productItemId;
            Price = price;
            StockQuantity = stockQuantity;
            IsActive = isActive;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public int SellerId { get; set; }
        public int ProductItemId { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
