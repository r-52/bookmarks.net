namespace Beartrail.Application.Mediator.Auth.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(p => p.Email)
            .NotNull()
            .NotEmpty()
            .MinimumLength(1)
            .EmailAddress();
        RuleFor(p => p.Password)
            .NotEmpty()
            .NotNull()
            .MinimumLength(1);
    }
}
