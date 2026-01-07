namespace Jannara_Ecommerce.DTOs.Role
{
    public class RoleDetailsDTO
    {
        public int Id { get; set; }

        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
