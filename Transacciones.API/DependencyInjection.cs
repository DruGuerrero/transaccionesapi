using Microsoft.Extensions.DependencyInjection;

namespace Transacciones.API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services)
    {
        // Register Core services here (use cases, validators, etc.)

        return services;
    }
}
