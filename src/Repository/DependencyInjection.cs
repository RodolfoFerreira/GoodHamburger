using Microsoft.Extensions.DependencyInjection;
using GoodHamburger.Repository.Interfaces;
using GoodHamburger.Repository.Repositories;

namespace Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            _ = services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
