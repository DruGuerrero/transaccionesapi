using System.Text.Json.Serialization;

namespace Transacciones.Core.Models.Account.CreateAccount;

public record CreateAccountRequest(
    Guid Id,
    [property: JsonPropertyName("numeroCuenta")] string AccountNumber,
    [property: JsonPropertyName("saldo")] decimal Balance,
    [property: JsonPropertyName("titular")] string Holder,
    [property: JsonPropertyName("fechaCreacion")] DateTime CreatedAt,
    [property: JsonPropertyName("activa")] bool IsActive
);
