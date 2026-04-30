using AutoMapper;
using Transacciones.Core.Entities.Account;
using Transacciones.Core.Models.Account;
using Transacciones.Core.Models.Account.CreateAccount;

namespace Transacciones.Core.Mappings;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<CreateAccountRequest, Accounts>()
            .ForMember(account => account.CreatedAt, opt => opt.Ignore());

        CreateMap<Accounts, CreateAccountResponse>();

        CreateMap<Accounts, AccountResponse>()
            .ConstructUsing(srcAccount => new AccountResponse(
                srcAccount.Id,
                srcAccount.AccountNumber,
                srcAccount.Balance,
                srcAccount.Holder,
                srcAccount.CreatedAt,
                srcAccount.IsActive
            ));
    }
}
