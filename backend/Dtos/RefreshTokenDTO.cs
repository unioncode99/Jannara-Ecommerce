namespace Jannara_Ecommerce.DTOs
{
    public class RefreshTokenDTO
    {
        public RefreshTokenDTO(int id, int userId, string token, DateTime createAt, DateTime expires)
        {
            Id = id;
            UserId = userId;
            Token = token;
            CreateAt = createAt;
            Expires = expires;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime Expires {  get; set; }
    }
}
