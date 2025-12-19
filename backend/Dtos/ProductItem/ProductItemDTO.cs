namespace Jannara_Ecommerce.DTOs.ProductItem
{
    public class ProductItemDTO
    {
        public ProductItemDTO(int id, int productId, string sku, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            ProductId = productId;
            Sku = sku;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public int ProductId {  get; set; }
        public string Sku {  get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
