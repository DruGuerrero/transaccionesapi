namespace Transacciones.Core.Models.Transaction;

public record TransactionResponse(
    Guid Id,
    Guid AccountId,
    string TransactionType,
    decimal Amount,
    DateTime TransactionDate,
    string Description,
    decimal PreviousBalance,
    decimal NewBalance
);
