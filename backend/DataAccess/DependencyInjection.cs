
using Jannara_Ecommerce.DataAccess.Interfaces;
using Jannara_Ecommerce.DataAccess.Repositories;

namespace Jannara_Ecommerce.DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationRepositories (this IServiceCollection services)
        {
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISellerRepository, SellerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IConfirmationTokenRepository, ConfirmationTokenRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            return services;
        }
    }
}
