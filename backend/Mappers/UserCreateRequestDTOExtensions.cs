using Jannara_Ecommerce.Business.DTOs.Person;
using Jannara_Ecommerce.Business.DTOs.User;
using Jannara_Ecommerce.Dtos.Customer;

namespace Jannara_Ecommerce.Business.Mappers
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
