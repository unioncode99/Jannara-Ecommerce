namespace Jannara_Ecommerce.DTOs.Authentication
{
    public class ResetPasswordDTO
    {
        public string? Token { get; set; }
        public string NewPassword { get; set; }
        public string? Code { get; set; }
    }
}
