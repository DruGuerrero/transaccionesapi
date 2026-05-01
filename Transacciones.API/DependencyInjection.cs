using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Transacciones.Core.Interfaces.Account;
using Transacciones.Core.Interfaces.Transaction;
using Transacciones.Core.Mappings;
using Transacciones.Core.UseCases.Account;
using Transacciones.Core.UseCases.Transaction;
using Transacciones.Core.Validators.Account;
using Transacciones.Core.Validators.Transaction;

namespace Transacciones.API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services)
    {
        services.AddScoped<ICreateAccountUseCase, CreateAccountUseCase>();
        services.AddScoped<IGetAccountByIdUseCase, GetAccountByIdUseCase>();
        services.AddScoped<IMakeDepositUseCase, MakeDepositUseCase>();
        services.AddScoped<IMakeWithdrawalUseCase, MakeWithdrawalUseCase>();
        services.AddScoped<IListTransactionsByAccountUseCase, ListTransactionsByAccountUseCase>();

        services.AddValidatorsFromAssemblyContaining<CreateAccountValidator>();
        services.AddValidatorsFromAssemblyContaining<MakeDepositRequestValidator>();

        services.AddAutoMapper(cfg => cfg.AddProfile<AccountMappingProfile>());
        services.AddAutoMapper(cfg => cfg.AddProfile<TransactionMappingProfile>());

        return services;
    }
}
