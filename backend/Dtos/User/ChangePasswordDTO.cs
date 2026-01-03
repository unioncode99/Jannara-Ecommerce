namespace Jannara_Ecommerce.DTOs.User
{
    public class ChangePasswordDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public int? UserId { get; set; } 

    }
}
