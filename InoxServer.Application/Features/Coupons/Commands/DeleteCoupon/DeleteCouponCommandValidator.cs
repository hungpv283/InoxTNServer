using FluentValidation;

namespace InoxServer.Application.Features.Coupons.Commands.DeleteCoupon;

public class DeleteCouponCommandValidator : AbstractValidator<DeleteCouponCommand>
{
    public DeleteCouponCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID không được để trống.");
    }
}
