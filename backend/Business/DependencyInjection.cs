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
            services.AddScoped<IConfirmationTokenService, ConfirmationTokenService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.AddScoped<IConfirmationService, ConfirmationService>();
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerWishlistService, CustomerWishlistService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartItemService, CartItemService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<IShippingMethodService, ShippingMethodService>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            services.AddScoped<IOrderService, OrderService>();
            return services;
        }
    }
}
