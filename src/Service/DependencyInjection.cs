using Microsoft.Extensions.DependencyInjection;
using GoodHamburger.Service.Services;
using GoodHamburger.Service.Interfaces;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}
