using System.Threading;
using System.Threading.Tasks;
using Transacciones.Core.Models.Transaction.MakeWithdrawal;

namespace Transacciones.Core.Interfaces.Transaction
{
    public interface IMakeWithdrawalUseCase
    {
        Task<MakeWithdrawalResponse> ExecuteAsync(MakeWithdrawalRequest request, CancellationToken cancellationToken = default);
    }
}
