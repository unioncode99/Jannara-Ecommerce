namespace Jannara_Ecommerce.Dtos
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
        public int PersonId { get; set; }
        public string Street {  get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
