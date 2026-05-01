using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Transacciones.Core.Interfaces.Account;
using Transacciones.Core.Interfaces.Transaction;
using Transacciones.Core.Models.Account.CreateAccount;
using Transacciones.Core.Models.Transaction.MakeDeposit;

namespace Transacciones.API.Endpoints
{
    [Route("/api/transacciones/abono")]
    public class MakeDepositEndpoint : EndpointBaseAsync.WithRequest<MakeDepositRequest>.WithActionResult
    {
        private readonly IMakeDepositUseCase _makeDepositUseCase;
        private readonly IValidator<MakeDepositRequest> _validator;

        public MakeDepositEndpoint(IMakeDepositUseCase makeDepositUseCase, IValidator<MakeDepositRequest> validator)
        {
            _makeDepositUseCase = makeDepositUseCase;
            _validator = validator;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Realiza un abono de dinero a una cuenta dado CuentaId",
            Description = "Realiza una transacción de tipo ABONO para la cuenta dada por medio del Id de cuenta.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public override async Task<ActionResult> HandleAsync(MakeDepositRequest request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            try
            {
                var result = await _makeDepositUseCase.ExecuteAsync(request, cancellationToken);
                return Created($"/api/transacciones/{result.Id}", result);
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
