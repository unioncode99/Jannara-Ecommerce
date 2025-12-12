using Jannara_Ecommerce.Enums;
using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs.Seller
{
    public class SellerCreateRequestDTO
    {
        // sellere inof 
        [Required(ErrorMessage = "BusinessName is required.")]
        public string BusinessName { get; set; }
        public string? WebsiteUrl { get; set; }
        // user info 
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email format is invalid.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        // perons info 
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
    }
}
