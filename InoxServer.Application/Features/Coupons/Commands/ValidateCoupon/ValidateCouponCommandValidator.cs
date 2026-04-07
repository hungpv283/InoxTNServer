using FluentValidation;

namespace InoxServer.Application.Features.Coupons.Commands.ValidateCoupon;

public class ValidateCouponCommandValidator : AbstractValidator<ValidateCouponCommand>
{
    public ValidateCouponCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Mã giảm giá không được để trống.")
            .MaximumLength(50)
            .WithMessage("Mã giảm giá không được vượt quá 50 ký tự.");

        RuleFor(x => x.OrderAmount)
            .GreaterThan(0)
            .WithMessage("Giá trị đơn hàng phải lớn hơn 0.");
    }
}