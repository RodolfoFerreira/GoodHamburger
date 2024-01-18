using Microsoft.Extensions.DependencyInjection;
using GoodHamburger.Service.Services;
using GoodHamburger.Service.Interfaces;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            _ = services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
