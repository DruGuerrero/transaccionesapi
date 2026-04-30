using Ardalis.Specification;
using Transacciones.Core.Entities.Account;

namespace Transacciones.Core.Entities.Account.Specifications;

public class AccountByIdSpec : Specification<Accounts>, ISingleResultSpecification<Accounts>
{
    public AccountByIdSpec(Guid id)
    {
        Query.Where(a => a.Id == id);
    }
}
