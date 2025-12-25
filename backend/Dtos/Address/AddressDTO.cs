using System.ComponentModel.DataAnnotations;

namespace Jannara_Ecommerce.DTOs.Address
{
    public class AddressDTO
    {
        public AddressDTO(int id, int personId, string state, string city, string locality, string street, string buildingNumber, string? phone, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            PersonId = personId;
            State = state;
            City = city;
            Locality = locality;
            Street = street;
            BuildingNumber = buildingNumber;
            Phone = phone;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public int PersonId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Locality {  get; set; }
        public string Street {  get; set; }
        public string BuildingNumber {  get; set; }
        public string? Phone {  get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
