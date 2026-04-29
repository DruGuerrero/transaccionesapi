using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transacciones.Core.Models.Account.CreateAccount;

namespace Transacciones.Core.Interfaces.Account
{
    public interface ICreateAccountUseCase
    {
        Task<CreateAccountResponse> ExecuteAsync(CreateAccountRequest request, CancellationToken cancellationToken = default);
    }
}
