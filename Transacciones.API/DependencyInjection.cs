using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Transacciones.Core.Interfaces.Account;
using Transacciones.Core.Mappings;
using Transacciones.Core.UseCases.Account;
using Transacciones.Core.Validators.Account;

namespace Transacciones.API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services)
    {
        services.AddScoped<ICreateAccountUseCase, CreateAccountUseCase>();
        services.AddScoped<IGetAccountByIdUseCase, GetAccountByIdUseCase>();

        services.AddValidatorsFromAssemblyContaining<CreateAccountValidator>();

        services.AddAutoMapper(cfg => cfg.AddProfile<AccountMappingProfile>());

        return services;
    }
}
