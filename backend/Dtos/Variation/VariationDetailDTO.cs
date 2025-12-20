using Jannara_Ecommerce.DTOs.VariationOption;

namespace Jannara_Ecommerce.DTOs.Variation
{
    public class VariationDetailDTO
    {
        public int VariationId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<VariationOptionDetailDTO> Options { get; set; }
    }
}
