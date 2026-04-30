using AutoMapper;
using Transacciones.Core.Entities.Account;
using Transacciones.Core.Models.Account;
using Transacciones.Core.Models.Account.CreateAccount;

namespace Transacciones.Core.Mappings;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<CreateAccountRequest, Accounts>();

        CreateMap<Accounts, AccountResponse>()
            .ConstructUsing(src => new AccountResponse(
                src.Id,
                src.AccountNumber,
                src.Balance,
                src.Holder,
                src.CreatedAt,
                src.IsActive
            ));
    }
}
