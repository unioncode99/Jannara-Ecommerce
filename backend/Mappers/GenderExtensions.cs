using Jannara_Ecommerce.Enums;

namespace Jannara_Ecommerce.Mappers
{
    public static class GenderExtensions
    {
        public static string ToArabic(this Gender gender)
        {
            return gender switch
            {
                Gender.Unknown => "غير معروف",
                Gender.Male => "ذكر",
                Gender.Female => "أنثى",
                Gender.Other => "آخر",
                _ => throw new NotImplementedException($"Arabic translation not implemented for {gender}")
            };
        }
    }
}
