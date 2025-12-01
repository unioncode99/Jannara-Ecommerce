using Jannara_Ecommerce.Business.Interfaces;
using Jannara_Ecommerce.Business.Services;

namespace Jannara_Ecommerce.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ISellerService, SellerService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
