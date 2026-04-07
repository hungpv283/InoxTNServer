using FluentValidation;
using InoxServer.Domain.Enums;

namespace InoxServer.Application.Features.Coupons.Commands.UpdateCoupon;

public class UpdateCouponCommandValidator : AbstractValidator<UpdateCouponCommand>
{
    public UpdateCouponCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID không được để trống.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Mã giảm giá không được để trống.")
            .MaximumLength(50)
            .WithMessage("Mã giảm giá không được vượt quá 50 ký tự.")
            .Matches(@"^[A-Z0-9]+$")
            .WithMessage("Mã giảm giá chỉ được chứa chữ hoa và số.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Loại coupon không hợp lệ.");

        RuleFor(x => x.Value)
            .GreaterThan(0)
            .WithMessage("Giá trị giảm giá phải lớn hơn 0.");

        RuleFor(x => x.MinOrderValue)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Giá trị đơn hàng tối thiểu không được âm.");

        RuleFor(x => x.MaxDiscount)
            .GreaterThan(0)
            .When(x => x.MaxDiscount.HasValue)
            .WithMessage("Giảm giá tối đa phải lớn hơn 0.");

        RuleFor(x => x.UsageLimit)
            .GreaterThan(0)
            .When(x => x.UsageLimit.HasValue)
            .WithMessage("Số lượng sử dụng phải lớn hơn 0.");

        RuleFor(x => x.ExpiresAt)
            .GreaterThan(x => x.StartsAt)
            .When(x => x.StartsAt.HasValue && x.ExpiresAt.HasValue)
            .WithMessage("Ngày hết hạn phải sau ngày bắt đầu.");
    }
}
