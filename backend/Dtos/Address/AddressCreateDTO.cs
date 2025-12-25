namespace Jannara_Ecommerce.DTOs.Address
{
    public class AddressCreateDTO
    {
        public int PersonId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Locality { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string? Phone { get; set; }
    }
}
