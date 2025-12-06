using Jannara_Ecommerce.DTOs.Customer;
using Jannara_Ecommerce.DTOs.Person;
using Jannara_Ecommerce.DTOs.User;

namespace Jannara_Ecommerce.Mappers
{
    public static class CustomerCreateRequestDTOExtensions
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
