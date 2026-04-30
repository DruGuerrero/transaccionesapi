using Ardalis.Specification;
using Transacciones.Core.Entities.Account;

namespace Transacciones.Core.Entities.Account.Specifications;

public class AccountByNumberSpec : Specification<Accounts>
{
    public AccountByNumberSpec(string accountNumber)
    {
        Query.Where(a => a.AccountNumber == accountNumber);
    }
}
