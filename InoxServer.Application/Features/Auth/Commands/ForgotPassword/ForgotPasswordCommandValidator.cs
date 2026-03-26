using FluentValidation;
using InoxServer.Domain.Errors;

namespace InoxServer.Application.Features.Auth.Commands.ForgotPassword;

public sealed class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.User.EmptyEmail)
            .WithMessage("Email không được để trống.")
            .EmailAddress()
            .WithErrorCode(DomainErrorCodes.User.InvalidEmail)
            .WithMessage("Email không đúng định dạng.");
    }
}
