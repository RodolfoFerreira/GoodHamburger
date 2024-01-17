using Microsoft.Extensions.DependencyInjection;
using Service.Services;
using Service.Interfaces;

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
