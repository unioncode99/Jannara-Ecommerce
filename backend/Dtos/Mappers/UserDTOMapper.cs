using Jannara_Ecommerce.DTOs.User;

namespace Jannara_Ecommerce.DTOs.Mappers
{
    public static class UserDTOMapper
    {
        public static UserPublicDTO ToUserPublicDTO(this UserDTO userDTO)
        {
            return new UserPublicDTO(userDTO.Id, userDTO.PersonId, userDTO.Email, userDTO.Username,userDTO.Roles, userDTO.CreatedAt, userDTO.UpdatedAt);
        }
    }
}
