using Microsoft.Extensions.DependencyInjection;

namespace Transacciones.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register Infrastructure layer services here (e.g. DbContext, repositories)

        return services;
    }
}
