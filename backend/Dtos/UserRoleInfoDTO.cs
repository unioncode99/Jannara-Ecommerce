namespace Jannara_Ecommerce.DTOs
{
    public class UserRoleInfoDTO
    {
        public UserRoleInfoDTO(int id, string nameAr, 
            string nameEn, bool isActive, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            NameAr = nameAr;
            NameEn = nameEn;
            IsActive = isActive;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public string NameAr {  get; set; }
        public string NameEn {  get; set; }
        public bool IsActive {  get; set; }
        public DateTime CreatedAt {  get; set; }
        public DateTime UpdatedAt {  get; set; }
    }
}
