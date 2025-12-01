using Jannara_Ecommerce.Enums;

namespace Jannara_Ecommerce.DTOs
{
    public class PersonDTO
    {
        public PersonDTO(int id, string firstName, string lastName, string phone, string? imageUrl, IFormFile? profileImage,
            Gender gender, DateOnly dateOfBirth, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            ImageUrl = imageUrl;
            ProfileImage = profileImage;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public Gender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
