using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs.ProductCategory
{
    public class ProductCategoryCreateDTO
    {
        public int? ParentCategoryId { get; set; }
        [Required(ErrorMessage = "NameEn is required.")]
        public string NameEn { get; set; }
        [Required(ErrorMessage = "NameAr is required.")]
        public string NameAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
    }
}
