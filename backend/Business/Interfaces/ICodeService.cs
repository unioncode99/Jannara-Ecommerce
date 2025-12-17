namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface ICodeService
    {
        public string GenerateCode(int length = 6);
        public string GenerateAlphaNumericCode(int length = 8);
    }
}
