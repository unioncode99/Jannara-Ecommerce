namespace Jannara_Ecommerce.Utilities
{
    public class JwtOptions
    {
        public string SigningKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int LifeTime { get; set; }
    }
}
