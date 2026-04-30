using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Transacciones.Core.Interfaces.Account;
using Transacciones.Core.Models.Account;

namespace Transacciones.API.Endpoints;

[Route("/api/cuentas")]
public class GetAccountByIdEndpoint : EndpointBaseAsync.WithRequest<Guid>.WithActionResult<AccountResponse>
{
    private readonly IGetAccountByIdUseCase _getAccountByIdUseCase;

    public GetAccountByIdEndpoint(IGetAccountByIdUseCase getAccountByIdUseCase)
    {
        _getAccountByIdUseCase = getAccountByIdUseCase;
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Obtener una cuenta por ID",
        Description = "Devuelve los detalles de una cuenta específica.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override async Task<ActionResult<AccountResponse>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _getAccountByIdUseCase.ExecuteAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
