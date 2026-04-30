namespace Transacciones.Core.Models.Account;

public record AccountResponse(
    Guid Id,
    string AccountNumber,
    decimal Balance,
    string Holder,
    DateTime CreatedAt,
    bool IsActive
);
