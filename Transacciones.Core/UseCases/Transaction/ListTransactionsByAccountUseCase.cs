using AutoMapper;
using Transacciones.Core.Entities.Transaction;
using Transacciones.Core.Entities.Transaction.Specifications;
using Transacciones.Core.Interfaces;
using Transacciones.Core.Interfaces.Transaction;
using Transacciones.Core.Models.Transaction;

namespace Transacciones.Core.UseCases.Transaction;

public class ListTransactionsByAccountUseCase : IListTransactionsByAccountUseCase
{
    private readonly IReadRepository<Transactions> _transactionRepository;
    private readonly IMapper _mapper;

    public ListTransactionsByAccountUseCase(IReadRepository<Transactions> transactionRepository, IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TransactionResponse>> ExecuteAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        var spec = new TransactionsByAccountIdSpec(accountId);
        var transactions = await _transactionRepository.ListAsync(spec, cancellationToken);
        
        return _mapper.Map<IEnumerable<TransactionResponse>>(transactions);
    }
}
