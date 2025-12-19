namespace Jannara_Ecommerce.DTOs.Variation
{
    public class VariationDTO
    {
        public VariationDTO(int id, int productId, string nameEn, string nameAr, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            ProductId = productId;
            NameEn = nameEn;
            NameAr = nameAr;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
