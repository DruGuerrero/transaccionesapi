using System.Text.Json.Serialization;

namespace Transacciones.Core.Models.Transaction;

public record TransactionResponse(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("cuentaId")] Guid AccountId,
    [property: JsonPropertyName("tipoTransaccion")] string TransactionType,
    [property: JsonPropertyName("monto")] decimal Amount,
    [property: JsonPropertyName("fechaTransaccion")] DateTime TransactionDate,
    [property: JsonPropertyName("descripcion")] string Description,
    [property: JsonPropertyName("saldoAnterior")] decimal PreviousBalance,
    [property: JsonPropertyName("nuevoSaldo")] decimal NewBalance
);
