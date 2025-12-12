using Jannara_Ecommerce.DTOs.User;

namespace Jannara_Ecommerce.Mappers
{
    public static class UserDTOExtensions
    {
        public static UserPublicDTO ToUserPublicDTO(this UserDTO userDTO)
        {
            return new UserPublicDTO(userDTO.Id, userDTO.PersonId, userDTO.Email, userDTO.Username, userDTO.CreatedAt, userDTO.UpdatedAt, userDTO.Roles);
        }
    }
}
