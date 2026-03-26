using FluentValidation;
using InoxServer.Domain.Errors;

namespace InoxServer.Application.Features.Cart.Commands.CheckoutCart;

public sealed class CheckoutCartCommandValidator : AbstractValidator<CheckoutCartCommand>
{
    public CheckoutCartCommandValidator()
    {
        RuleFor(x => x.ShippingFee)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode(DomainErrorCodes.General.InvalidOperation)
            .WithMessage("Phí vận chuyển không hợp lệ.");

        RuleFor(x => x.ShippingName)
            .NotEmpty()
            .MaximumLength(100)
            .WithErrorCode(DomainErrorCodes.General.InvalidOperation)
            .WithMessage("Tên người nhận không hợp lệ.");

        RuleFor(x => x.ShippingPhone)
            .NotEmpty()
            .MaximumLength(20)
            .WithErrorCode(DomainErrorCodes.General.InvalidOperation)
            .WithMessage("Số điện thoại không hợp lệ.");

        RuleFor(x => x.ShippingAddress)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.General.InvalidOperation)
            .WithMessage("Địa chỉ giao hàng không được để trống.");

        RuleFor(x => x.ShippingProvince)
            .MaximumLength(100)
            .WithErrorCode(DomainErrorCodes.General.InvalidOperation)
            .WithMessage("Tỉnh/thành phố không hợp lệ.");
    }
}

