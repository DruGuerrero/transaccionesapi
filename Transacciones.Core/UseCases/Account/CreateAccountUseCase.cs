using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transacciones.Core.Models.Account.CreateAccount;

namespace Transacciones.Core.UseCases.Account
{
    public class CreateAccountUseCase
    {
        public Task<CreateAccountResponse> ExecuteAsync(CreateAccountRequest request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new CreateAccountResponse(
                request.Id,
                request.AccountNumber,
                request.Balance,
                request.Holder,
                request.CreatedAt,
                request.IsActive
            ));
        }

    }
}
