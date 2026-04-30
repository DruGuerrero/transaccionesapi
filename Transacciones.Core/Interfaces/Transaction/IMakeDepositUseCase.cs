using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transacciones.Core.Models.Account.CreateAccount;
using Transacciones.Core.Models.Transaction.MakeDeposit;

namespace Transacciones.Core.Interfaces.Transaction
{
    public interface IMakeDepositUseCase
    {
        Task<MakeDepositResponse> ExecuteAsync(MakeDepositRequest request, CancellationToken cancellationToken = default);

    }
}
