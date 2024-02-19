using FluentValidation;

namespace App.Application.Accounts.Commands
{
    public class LoginAccountValidator : AbstractValidator<LoginAccountCommand>
    {
        public LoginAccountValidator()
        {
            RuleFor(r => r.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Requested value is not an email address.");

            RuleFor(r => r.Password)
                .NotEmpty()
                .WithMessage("Password is required.");
        }
    }
}
