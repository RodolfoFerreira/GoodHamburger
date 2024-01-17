using Infrastructure.DatabaseContext;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            _ = services.AddDbContext<ApplicationDbContext>();

            _ = services.AddSingleton(TimeProvider.System);

            return services;
        }
    }
}
