using Jannara_Ecommerce.Business.Interfaces;

namespace Jannara_Ecommerce.Business.Services
{
    public class CodeService : ICodeService
    {
        public string GenerateCode(int length = 4)
        {
            var random = new Random();
            var code = "";
            for (int i = 0; i < length; i++)
                code += random.Next(0, 10); // generates digits 0-9
            return code;
        }

        public string GenerateAlphaNumericCode(int length = 4)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
