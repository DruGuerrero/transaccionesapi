using Transacciones.Core.Models.Account;

namespace Transacciones.Core.Interfaces.Account;

public interface IGetAccountByIdUseCase
{
    Task<AccountResponse> ExecuteAsync(Guid id, CancellationToken cancellationToken = default);
}
