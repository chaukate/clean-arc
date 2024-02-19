using FluentValidation;

namespace App.Application.Accounts.Commands
{
    public class RequestPasswordChangeValidator : AbstractValidator<RequestPasswordChangeCommand>
    {
        public RequestPasswordChangeValidator()
        {
            RuleFor(r => r.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email.");

            RuleFor(r => r.ClientUrl)
                .NotEmpty()
                .WithMessage("Client url is required.");
        }
    }
}
