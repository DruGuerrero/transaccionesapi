using FluentValidation;
using Transacciones.Core.Models.Account.CreateAccount;

namespace Transacciones.Core.Validators.Account;

public class CreateAccountValidator : AbstractValidator<CreateAccountRequest>
{
    public CreateAccountValidator()
    {
        RuleFor(x => x.AccountNumber)
            .NotEmpty().WithMessage("El número de cuenta es obligatorio.")
            .Length(10).WithMessage("El número de cuenta debe tener 10 caracteres.");

        RuleFor(x => x.Balance)
            .GreaterThanOrEqualTo(0).WithMessage("El saldo inicial no puede ser negativo.");

        RuleFor(x => x.Holder)
            .NotEmpty().WithMessage("el titular es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre del titular no puede exceder los 100 caracteres.");
    }
}
