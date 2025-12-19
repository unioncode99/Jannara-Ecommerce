namespace Jannara_Ecommerce.DTOs.ProductItemVariationOption
{
    public class ProductItemVariationOptionDTO
    {
        public ProductItemVariationOptionDTO(int id, int variationOptionId, int productItemId, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            VariationOptionId = variationOptionId;
            ProductItemId = productItemId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public int VariationOptionId { get; set; }
        public int ProductItemId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
