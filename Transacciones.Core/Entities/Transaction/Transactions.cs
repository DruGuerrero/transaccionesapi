using Transacciones.Core.Entities.Account;

namespace Transacciones.Core.Entities.Transaction
{
    public class Transactions
    {
        public Guid Id { get; set; }
        public required Guid AccountId { get; set; }
        public required string TransactionType { get; set; } // ABONO o RETIRO 
        public required decimal Amount { get; set; }
        public required DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public required string Description { get; set; }

        public Accounts Account { get; set; } = null!;
    }
}
