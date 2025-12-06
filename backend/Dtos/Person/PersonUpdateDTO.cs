using Jannara_Ecommerce.Enums;
using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs.Person
{
    public class PersonUpdateDTO
    {
        [Required(ErrorMessage = "Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage ="Id must be greater than zero")]
        public int Id { get; set; }
        [Required(ErrorMessage = "FirstName is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Phone is required.")]
        [MaxLength(15, ErrorMessage = "Phone number cannot exceed 15 digits.")]
        public string Phone { get; set; }
        public IFormFile? ProfileImage { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }
        [Required(ErrorMessage = "DateOfBirth is required.")]
        public DateOnly DateOfBirth { get; set; }
        [Required(ErrorMessage = "DeleteProfileImage is required.")]
        public bool DeleteProfileImage { get; set; }
    }
}
