using Jannara_Ecommerce.Enums;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "FirstName is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Phone is required.")]
        [MaxLength(15, ErrorMessage = "Phone number cannot exceed 15 digits.")]
        public string Phone { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? ProfileImage { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }
        [Required(ErrorMessage = "DateOfBirth is required.")]
        public DateOnly DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
