using FluentValidation;
using InoxServer.Domain.Errors;

namespace InoxServer.Application.Features.Auth.Commands.ChangePassword;

public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.User.EmptyPassword)
            .WithMessage("Mật khẩu hiện tại không được để trống.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.User.EmptyPassword)
            .WithMessage("Mật khẩu mới không được để trống.")
            .MinimumLength(6)
            .WithErrorCode(DomainErrorCodes.User.ShortPassword)
            .WithMessage("Mật khẩu mới phải có ít nhất 6 ký tự.")
            .MaximumLength(100)
            .WithErrorCode(DomainErrorCodes.User.LongPassword)
            .WithMessage("Mật khẩu mới không được vượt quá 100 ký tự.");
    }
}
