namespace Jannara_Ecommerce.DTOs.Mapper
{
    public static class RegisteredCustomerDTOMapper
    {
        public static PersonDTO GetPersonDTO(this RegisteredCustomerDTO registerCustomerDTO)
        {
            return new PersonDTO(-1, registerCustomerDTO.FirstName, registerCustomerDTO.LastName, registerCustomerDTO.Phone, null, registerCustomerDTO.Gender, registerCustomerDTO.DateOfBirth, DateTime.Now, DateTime.Now);
        }
        public static UserDTO GetUserDTO(this RegisteredCustomerDTO registerCustomerDTO, int PersonId)
        {
            return new UserDTO(-1, PersonId, registerCustomerDTO.Email, registerCustomerDTO.Username, registerCustomerDTO.Password, DateTime.Now, DateTime.Now);
        }
    }
}
