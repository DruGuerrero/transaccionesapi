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
            Task.CompletedTask.Wait();
            return Task.FromResult(new CreateAccountResponse
            {
                Id = request.Id,
                AccountNumber = request.AccountNumber,
                Balance = request.Balance,
                Holder = request.Holder,
                CreatedAt = request.CreatedAt,
                IsActive = request.IsActive
            });
        }

    }
}
