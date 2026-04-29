using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Transacciones.Core.Interfaces.Account;
using Transacciones.Core.Models.Account.CreateAccount;

namespace Transacciones.API.Endpoints
{
    [Route("/cuentas")]
    public class CreateAccountEndpoint : EndpointBaseAsync.WithRequest<CreateAccountRequest>.WithActionResult
    {
        private readonly ICreateAccountUseCase _createAccountUseCase;

        public CreateAccountEndpoint(ICreateAccountUseCase createAccountUseCase)
        {
            _createAccountUseCase = createAccountUseCase;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Crear una nueva cuenta",
            Description = "Crea una nueva cuenta para realizar transacciones.",
            Tags = ["Cuentas"])]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public override async Task<ActionResult> HandleAsync(CreateAccountRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _createAccountUseCase.ExecuteAsync(request, cancellationToken);
                return Created($"/cuentas/{result.Id}", new CreateAccountResponse(result.Id, result.AccountNumber, result.Balance, result.Holder, result.CreatedAt, result.IsActive));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
