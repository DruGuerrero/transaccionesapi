using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Transacciones.Core.Interfaces.Transaction;
using Transacciones.Core.Models.Transaction.MakeWithdrawal;

namespace Transacciones.API.Endpoints
{
    [Route("/api/transacciones/retiro")]
    public class MakeWithdrawalEndpoint : EndpointBaseAsync.WithRequest<MakeWithdrawalRequest>.WithActionResult
    {
        private readonly IMakeWithdrawalUseCase _makeWithdrawalUseCase;
        private readonly IValidator<MakeWithdrawalRequest> _validator;

        public MakeWithdrawalEndpoint(IMakeWithdrawalUseCase makeWithdrawalUseCase, IValidator<MakeWithdrawalRequest> validator)
        {
            _makeWithdrawalUseCase = makeWithdrawalUseCase;
            _validator = validator;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Realiza un retiro de dinero de una cuenta dado CuentaId",
            Description = "Realiza una transacción de tipo RETIRO para la cuenta dada por medio del Id de cuenta.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public override async Task<ActionResult> HandleAsync(MakeWithdrawalRequest request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }

            try
            {
                var result = await _makeWithdrawalUseCase.ExecuteAsync(request, cancellationToken);
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
