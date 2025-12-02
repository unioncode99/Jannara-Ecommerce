using Jannara_Ecommerce.Dtos.Customer;
using Jannara_Ecommerce.Dtos.Person;
using Jannara_Ecommerce.Dtos.User;
using Jannara_Ecommerce.DTOs.User;

namespace Jannara_Ecommerce.DTOs.Mappers
{
    public static class UserCreateRequestDTOMapper
    {
        public static PersonCreateDTO GetPersonCreateDTO(this UserCreateRequestDTO  userCreateRequestDTO)
        {
            return new PersonCreateDTO
            {
                FirstName = userCreateRequestDTO.FirstName,
                LastName = userCreateRequestDTO.LastName,
                Phone = userCreateRequestDTO.Phone,
                ProfileImage = userCreateRequestDTO.ProfileImage,
                Gender = userCreateRequestDTO.Gender,
                DateOfBirth = userCreateRequestDTO.DateOfBirth
            };
        }
        public static UserCreateDTO GetUserCreateDTO(this UserCreateRequestDTO userCreateRequestDTO)
        {
            return new UserCreateDTO
            {
                Email = userCreateRequestDTO.Email,
                Username = userCreateRequestDTO.Username,
                Password = userCreateRequestDTO.Password
            };
        }
    }
}
