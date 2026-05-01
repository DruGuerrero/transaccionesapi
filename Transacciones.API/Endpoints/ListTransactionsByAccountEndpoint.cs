using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Transacciones.Core.Interfaces.Transaction;
using Transacciones.Core.Models.Transaction;

namespace Transacciones.API.Endpoints;

[Route("/api/transacciones")]
public class ListTransactionsByAccountEndpoint : EndpointBaseAsync.WithRequest<Guid>.WithActionResult<IEnumerable<TransactionResponse>>
{
    private readonly IListTransactionsByAccountUseCase _listTransactionsUseCase;

    public ListTransactionsByAccountEndpoint(IListTransactionsByAccountUseCase listTransactionsUseCase)
    {
        _listTransactionsUseCase = listTransactionsUseCase;
    }

    [HttpGet("cuenta/{id:guid}")]
    [SwaggerOperation(
        Summary = "Obtener transacciones por cuenta",
        Description = "Devuelve una lista de transacciones asociadas a una cuenta específica.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public override async Task<ActionResult<IEnumerable<TransactionResponse>>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _listTransactionsUseCase.ExecuteAsync(id, cancellationToken);
        
        return Ok(result);
    }
}
