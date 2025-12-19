using Jannara_Ecommerce.DTOs.VariationOption;

namespace Jannara_Ecommerce.DTOs.Variation
{
    public class VariationCreateDTO
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public IEnumerable<VariationOptionCreateDTO> VariationOptions { get; set; }
    }
}
