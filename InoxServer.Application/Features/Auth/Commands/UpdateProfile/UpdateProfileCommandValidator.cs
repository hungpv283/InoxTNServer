using FluentValidation;
using InoxServer.Domain.Errors;

namespace InoxServer.Application.Features.Auth.Commands.UpdateProfile;

public sealed class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.User.EmptyName)
            .WithMessage("Họ tên không được để trống.")
            .MaximumLength(100)
            .WithMessage("Họ tên không được vượt quá 100 ký tự.");

        RuleFor(x => x.Phone)
            .MaximumLength(20)
            .WithMessage("Số điện thoại không được vượt quá 20 ký tự.")
            .When(x => x.Phone is not null);

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .WithMessage("Địa chỉ không được vượt quá 500 ký tự.")
            .When(x => x.Address is not null);
    }
}
