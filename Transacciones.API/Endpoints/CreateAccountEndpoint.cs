using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Transacciones.Core.Interfaces.Account;
using Transacciones.Core.Models.Account.CreateAccount;

namespace Transacciones.API.Endpoints
{
    [Route("/api/cuentas")]
    public class CreateAccountEndpoint : EndpointBaseAsync.WithRequest<CreateAccountRequest>.WithActionResult
    {
        private readonly ICreateAccountUseCase _createAccountUseCase;
        private readonly IValidator<CreateAccountRequest> _validator;
        private readonly ILogger<CreateAccountEndpoint> _logger;

        public CreateAccountEndpoint(ICreateAccountUseCase createAccountUseCase, IValidator<CreateAccountRequest> validator, ILogger<CreateAccountEndpoint> logger)
        {
            _createAccountUseCase = createAccountUseCase;
            _validator = validator;
            _logger = logger;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Crear una nueva cuenta",
            Description = "Crea una nueva cuenta para realizar transacciones.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public override async Task<ActionResult> HandleAsync(CreateAccountRequest request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            try
            {
                var result = await _createAccountUseCase.ExecuteAsync(request, cancellationToken);
                return Created($"/api/cuentas/{result.Id}", new CreateAccountResponse(result.Id, result.AccountNumber, result.Balance, result.Holder, result.CreatedAt, result.IsActive));
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
