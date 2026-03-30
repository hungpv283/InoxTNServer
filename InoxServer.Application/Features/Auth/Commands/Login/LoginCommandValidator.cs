using FluentValidation;
using InoxServer.Domain.Errors;

namespace InoxServer.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.User.EmptyEmail)
            .WithMessage("Email không được để trống.")
            .EmailAddress()
            .WithErrorCode(DomainErrorCodes.User.InvalidEmail)
            .WithMessage("Email không đúng định dạng.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.User.EmptyPassword)
            .WithMessage("Mật khẩu không được để trống.")
            .MinimumLength(6)
            .WithErrorCode(DomainErrorCodes.User.ShortPassword)
            .WithMessage("Mật khẩu phải có ít nhất 6 ký tự.");
    }
}
