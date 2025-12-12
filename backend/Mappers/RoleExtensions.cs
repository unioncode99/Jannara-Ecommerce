using Jannara_Ecommerce.Enums;

namespace Jannara_Ecommerce.Mappers
{
    public static class RoleExtensions
    {
        public static string GetNameAr(this Roles role)
        {
            return role switch
            {
                Roles.Admin => "مشرف",
                Roles.SuperAdmin => "مشرف عام",
                Roles.Customer => "عميل",
                Roles.Seller => "بائع",
                _ => throw new NotImplementedException($"Display name not implemented for role {role}")
            };
        }
    }
}
