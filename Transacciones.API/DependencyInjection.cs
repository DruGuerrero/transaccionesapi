using Microsoft.Extensions.DependencyInjection;
using Transacciones.Core.Interfaces.Account;
using Transacciones.Core.UseCases.Account;

namespace Transacciones.API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services)
    {
        services.AddScoped<ICreateAccountUseCase, CreateAccountUseCase>();

        return services;
    }
}
