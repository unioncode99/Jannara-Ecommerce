using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs
{
    public class ProductCategoryDTO
    {
        public ProductCategoryDTO(int id, string nameEn, string nameAr, string? descriptionEn, string? descriptionAr, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            NameEn = nameEn;
            NameAr = nameAr;
            DescriptionEn = descriptionEn;
            DescriptionAr = descriptionAr;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "NameEn is required.")]
        public string NameEn { get; set; }
        [Required(ErrorMessage = "NameAr is required.")]
        public string NameAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
