namespace Jannara_Ecommerce.DTOs.PaymentMethod
{
    public class PaymentMethodDTO
    {
        public PaymentMethodDTO(int id, string nameEn, string nameAr, string? descriptionEn, string? descriptionAr, bool isActive, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            NameEn = nameEn;
            NameAr = nameAr;
            DescriptionEn = descriptionEn;
            DescriptionAr = descriptionAr;
            IsActive = isActive;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
