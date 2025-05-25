using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Services.Implementations;
using Shop.Application.Services.Interfaces;
using Shop.Domain.Interfaces;
using Shop.Infra.Data.Repositories;

namespace Shop.Infra.IoC
{
    public class DependencyContainer
    {
        public static void RegisterService(IServiceCollection services)
        {
            #region Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISiteSettingService, SiteSettingService>();

            #endregion
            #region Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWalletRepositorry, WalletRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISiteSettingRepository,SiteSettingRepository>();

            #endregion
            #region Tools
            services.AddScoped<ISmsService,SmsService>();
            #endregion
        }
    }
}
