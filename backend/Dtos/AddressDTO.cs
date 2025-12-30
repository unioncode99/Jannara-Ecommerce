using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs
{
    public class AddressDTO
    {
        public AddressDTO(int id, int personId, string street, string city, string state, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            PersonId = personId;
            Street = street;
            City = city;
            State = state;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "PersonId is required.")]
        public int PersonId { get; set; }
        [Required(ErrorMessage = "Street is required.")]
        public string Street {  get; set; }
        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }
        public string State { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
