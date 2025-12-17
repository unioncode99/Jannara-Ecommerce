using Jannara_Ecommerce.Enums;

namespace Jannara_Ecommerce.DTOs.Token
{
    public class ConfirmationTokenDTO
    {
        public ConfirmationTokenDTO(int id, int userId, string token, string code, ConfirmationPurpose purpose, DateTime expireAt, bool isUsed)
        {
            Id = id;
            UserId = userId;
            Token = token;
            Code = code;
            Purpose = purpose;
            ExpireAt = expireAt;
            IsUsed = isUsed;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Code { get; set; }   
        public ConfirmationPurpose Purpose { get; set; }
        public DateTime ExpireAt { get; set; }
        public bool IsUsed { get; set; }
    }
}
