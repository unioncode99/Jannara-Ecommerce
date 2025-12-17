namespace Jannara_Ecommerce.DTOs.Token
{
    public class AccessTokenDTO
    {
        public AccessTokenDTO(string token, DateTime expires, DateTime issuedAt)
        {
            Token = token;
            Expires = expires;
            IssuedAt = issuedAt;
        }
        public string Token {  get; set; }
        public DateTime Expires { get; set; }
        public DateTime IssuedAt { get; set; }
    }
}
