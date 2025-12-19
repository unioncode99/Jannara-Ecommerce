namespace Jannara_Ecommerce.DTOs.VariationOption
{
    public class VariationOptionDTO
    {
        public VariationOptionDTO(int id, int variationId, string valueEn, string valueAr, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            VariationId = variationId;
            ValueEn = valueEn;
            ValueAr = valueAr;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public int VariationId { get; set; }
        public string ValueEn { get; set; }
        public string ValueAr { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
