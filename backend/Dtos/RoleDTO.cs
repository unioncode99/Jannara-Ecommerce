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
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
