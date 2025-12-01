namespace Jannara_Ecommerce.DTOs.Mapper
{
    public static class RegisteredSellerDTOMapper
    {
        public static PersonDTO GetPersonDTO(this RegisteredSellerDTO registerSellerDTO)
        {
            return new PersonDTO(-1, registerSellerDTO.FirstName, registerSellerDTO.LastName, registerSellerDTO.Phone, null, registerSellerDTO.Gender, registerSellerDTO.DateOfBirth, DateTime.Now, DateTime.Now);
        }
        public static UserDTO GetUserDTO(this RegisteredSellerDTO registerSellerDTO, int PersonId)
        {
            return new UserDTO(-1, PersonId, registerSellerDTO.Email, registerSellerDTO.Username, registerSellerDTO.Password, DateTime.Now, DateTime.Now);
        }
    }
}
