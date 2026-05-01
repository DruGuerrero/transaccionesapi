using Transacciones.Core.Models.Transaction;

namespace Transacciones.Core.Interfaces.Transaction;

public interface IListTransactionsByAccountUseCase
{
    Task<IEnumerable<TransactionResponse>> ExecuteAsync(Guid accountId, CancellationToken cancellationToken = default);
}
