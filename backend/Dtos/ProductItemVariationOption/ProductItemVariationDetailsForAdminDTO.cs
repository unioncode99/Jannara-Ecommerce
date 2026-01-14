namespace Jannara_Ecommerce.DTOs.ProductItemVariationOption
{
    public class ProductItemVariationDetailsForAdminDTO
    {
        public int Id { get; set; }
        public int variationOptionId { get; set; }
        public string ValueEn { get; set; }
        public string ValueAr { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
