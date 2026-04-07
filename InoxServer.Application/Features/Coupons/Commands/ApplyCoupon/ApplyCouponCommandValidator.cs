using FluentValidation;

namespace InoxServer.Application.Features.Coupons.Commands.ApplyCoupon;

public class ApplyCouponCommandValidator : AbstractValidator<ApplyCouponCommand>
{
    public ApplyCouponCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Mã giảm giá không được để trống.")
            .MaximumLength(50)
            .WithMessage("Mã giảm giá không được vượt quá 50 ký tự.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId không được để trống.");

        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("OrderId không được để trống.");

        RuleFor(x => x.OrderAmount)
            .GreaterThan(0)
            .WithMessage("Giá trị đơn hàng phải lớn hơn 0.");
    }
}