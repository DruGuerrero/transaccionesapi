using Ardalis.Specification;

namespace Transacciones.Core.Entities.Transaction.Specifications;

public class TransactionsByAccountIdSpec : Specification<Transactions>
{
    public TransactionsByAccountIdSpec(Guid accountId)
    {
        Query.Where(t => t.AccountId == accountId)
             .OrderByDescending(t => t.TransactionDate);
    }
}
