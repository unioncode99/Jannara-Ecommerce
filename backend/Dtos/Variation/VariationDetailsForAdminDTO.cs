using Jannara_Ecommerce.DTOs.VariationOption;

namespace Jannara_Ecommerce.DTOs.Variation
{
    public class VariationDetailsForAdminDTO
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<VariationOptionDetailsForAdminDTO> VariationOptions { get; set; }
    }
}
