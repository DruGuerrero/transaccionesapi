using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transacciones.Core.Entities.Account;
using Transacciones.Core.Interfaces;
using Transacciones.Core.Interfaces.Account;
using Transacciones.Core.Models.Account.CreateAccount;

namespace Transacciones.Core.UseCases.Account;

public class CreateAccountUseCase : ICreateAccountUseCase
{
    private readonly IRepository<Accounts> _repository;

    public CreateAccountUseCase(IRepository<Accounts> repository)
    {
        _repository = repository;
    }

    public async Task<CreateAccountResponse> ExecuteAsync(CreateAccountRequest request, CancellationToken cancellationToken = default)
    {
        var account = new Accounts
        {
            Id = request.Id,
            AccountNumber = request.AccountNumber,
            Balance = request.Balance,
            Holder = request.Holder,
            CreatedAt = request.CreatedAt,
            IsActive = request.IsActive
        };

        await _repository.AddAsync(account, cancellationToken);

        return new CreateAccountResponse(
            account.Id,
            account.AccountNumber,
            account.Balance,
            account.Holder,
            account.CreatedAt,
            account.IsActive
        );
    }
}
