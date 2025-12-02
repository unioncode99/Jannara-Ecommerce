using Jannara_Ecommerce.Dtos.Customer;
using Jannara_Ecommerce.Dtos.Person;
using Jannara_Ecommerce.Dtos.User;
using Jannara_Ecommerce.DTOs.Seller;

namespace Jannara_Ecommerce.DTOs.Mappers
{
    public static class SellerCreateRequestDTOMapper
    {
        public static PersonCreateDTO GetPersonCreateDTO(this SellerCreateRequestDTO  sellerCreateRequestDTO)
        {
            return new PersonCreateDTO
            {
                FirstName = sellerCreateRequestDTO.FirstName,
                LastName = sellerCreateRequestDTO.LastName,
                Phone = sellerCreateRequestDTO.Phone,
                ProfileImage = sellerCreateRequestDTO.ProfileImage,
                Gender = sellerCreateRequestDTO.Gender,
                DateOfBirth = sellerCreateRequestDTO.DateOfBirth
            };
        }
        public static UserCreateDTO GetUserCreateDTO(this SellerCreateRequestDTO sellerCreateRequestDTO)
        {
            return new UserCreateDTO
            {
                Email = sellerCreateRequestDTO.Email,
                Username = sellerCreateRequestDTO.Username,
                Password = sellerCreateRequestDTO.Password
            };
        }
        public static SellerCreateDTO GetSellerCreateDTO(this SellerCreateRequestDTO sellerCreateRequestDTO)
        {
            return new SellerCreateDTO
            {
                BusinessName = sellerCreateRequestDTO.BusinessName,
                WebsiteUrl = sellerCreateRequestDTO.WebsiteUrl
            };
        }
    }
}
