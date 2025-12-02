using Jannara_Ecommerce.Dtos.Customer;
using Jannara_Ecommerce.Dtos.Person;
using Jannara_Ecommerce.Dtos.User;
using Jannara_Ecommerce.DTOs;

namespace Jannara_Ecommerce.Dtos.Mappers
{
    public static class CustomerCreateRequestDTOMapper
    {
        public static PersonCreateDTO GetPersonCreateDTO(this CustomerCreateRequestDTO customerCreateRequestDTO)
        {
            return new PersonCreateDTO
            {
                FirstName = customerCreateRequestDTO.FirstName,
                LastName = customerCreateRequestDTO.LastName,
                Phone = customerCreateRequestDTO.Phone,
                ProfileImage = customerCreateRequestDTO.ProfileImage,
                Gender = customerCreateRequestDTO.Gender,
                DateOfBirth = customerCreateRequestDTO.DateOfBirth
            };
        }
        public static UserCreateDTO GetUserCreateDTO(this CustomerCreateRequestDTO customerCreateRequestDTO)
        {
            return new UserCreateDTO
            {
                Email = customerCreateRequestDTO.Email,
                Username = customerCreateRequestDTO.Username,
                Password = customerCreateRequestDTO.Password
            };
        }
    }
}
