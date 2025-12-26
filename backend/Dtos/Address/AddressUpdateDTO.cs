namespace Jannara_Ecommerce.DTOs.Address
{
    public class AddressUpdateDTO
    {
        public int StateId { get; set; }
        public string City { get; set; }
        public string Locality { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string? Phone { get; set; }
    }
}
