using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs
{
    public class RoleDTO
    {
        public RoleDTO(int id, string nameEn, string nameAr, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            NameEn = nameEn;
            NameAr = nameAr;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "NameEn is required.")]
        public string NameEn { get; set; }
        [Required(ErrorMessage = "NameAr is required.")]
        public string NameAr { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
