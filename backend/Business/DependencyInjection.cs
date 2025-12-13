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
            services.AddScoped<IUserConfirmationService, UserConfirmationService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICodeService, CodeService>();
            services.AddScoped<IConfirmationTokenServiceInterface, ConfirmationTokenService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.AddScoped<IAccountConfirmationServiceInterface, AccountConfirmationServiceInterface>();
            return services;
        }
    }
}
