using System.Text.Json.Serialization;

namespace Transacciones.Core.Models.Transaction.MakeDeposit;

public record MakeDepositRequest(
    [property: JsonPropertyName("cuentaId")] Guid AccountId,
    [property: JsonPropertyName("monto")] decimal Amount,
    [property: JsonPropertyName("descripcion")] string Description
);
