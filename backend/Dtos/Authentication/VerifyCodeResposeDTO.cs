using Jannara_Ecommerce.Enums;
using Jannara_Ecommerce.Utilities.WrapperClasses;

namespace Jannara_Ecommerce.DTOs.Authentication
{
    public class VerifyCodeResposeDTO
    {
        public VerifyCodeResposeDTO(ConfirmationPurpose purpose, string token)
        {
            Purpose = purpose;
            Token = token;
        }

        public ConfirmationPurpose Purpose {  get; set; }
        public string Token { get; set; } 
    }
}
