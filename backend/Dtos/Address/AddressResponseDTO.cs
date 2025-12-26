using Jannara_Ecommerce.DTOs.State;

namespace Jannara_Ecommerce.DTOs.Address
{
    public class AddressResponseDTO
    {
        public IEnumerable<AddressDTO> Addresses { get; set; }
        public IEnumerable<StateDTO> States { get; set; }
    }
}
