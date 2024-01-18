using Microsoft.Extensions.DependencyInjection;
using GoodHamburger.Repository.Interfaces;
using GoodHamburger.Repository.Repositories;

namespace Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderProductRepository, OrderProductRepository>();

            return services;
        }
    }
}
