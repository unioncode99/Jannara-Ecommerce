namespace Jannara_Ecommerce.DTOs.ShippingMethod
{
    public class ShippingMethodDTO
    {
        public ShippingMethodDTO(int id, string code, string nameEn, string nameAr, string? descriptionEn, string? descriptionAr, decimal basePrice, decimal pricePerKg, decimal freeOver, byte daysMin, byte daysMax, bool isActive, byte sortOrder, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            Code = code;
            NameEn = nameEn;
            NameAr = nameAr;
            DescriptionEn = descriptionEn;
            DescriptionAr = descriptionAr;
            BasePrice = basePrice;
            PricePerKg = pricePerKg;
            FreeOver = freeOver;
            DaysMin = daysMin;
            DaysMax = daysMax;
            IsActive = isActive;
            SortOrder = sortOrder;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public decimal BasePrice { get; set; }
        public decimal PricePerKg { get; set; }
        public decimal FreeOver { get; set; }
        public byte DaysMin { get; set; }
        public byte DaysMax { get; set; }
        public bool IsActive { get; set; }
        public byte SortOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
