using FluentValidation;
using Transacciones.Core.Models.Transaction.MakeWithdrawal;

namespace Transacciones.Core.Validators.Transaction;

public class MakeWithdrawalRequestValidator : AbstractValidator<MakeWithdrawalRequest>
{
    public MakeWithdrawalRequestValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("El ID de la cuenta es requerido.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("El monto debe ser mayor a 0.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción es requerida.");
    }
}
