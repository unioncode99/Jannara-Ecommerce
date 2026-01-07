namespace Jannara_Ecommerce.DTOs.Person
{
    public class PersonDetailsDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? Phone { get; set; }
        public string? ImageUrl { get; set; }

        public int Gender { get; set; }
        public string GenderNameEn { get; set; }
        public string GenderNameAr { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
