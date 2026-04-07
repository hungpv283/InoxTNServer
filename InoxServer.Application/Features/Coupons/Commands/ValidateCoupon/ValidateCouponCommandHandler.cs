using InoxServer.Application.Features.Coupons.DTOs;
using InoxServer.Domain.Enums;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading.Tasks;

namespace InoxServer.Application.Features.Coupons.Commands.ValidateCoupon;

public class ValidateCouponCommandHandler : IRequestHandler<ValidateCouponCommand, ValidateCouponResultDto>
{
    private readonly ICouponRepository _couponRepository;

    public ValidateCouponCommandHandler(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository;
    }

    public async Task<ValidateCouponResultDto> Handle(ValidateCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = await _couponRepository.GetByCodeAsync(request.Code.ToUpper().Trim(), cancellationToken);

        if (coupon == null)
            return CreateInvalidResult(coupon, "Mã giảm giá không tồn tại.");

        if (!coupon.IsActive)
            return CreateInvalidResult(coupon, "Mã giảm giá không còn hoạt động.");

        var now = DateTime.UtcNow;

        if (coupon.StartsAt.HasValue && coupon.StartsAt > now)
            return CreateInvalidResult(coupon, "Mã giảm giá chưa có hiệu lực.");

        if (coupon.ExpiresAt.HasValue && coupon.ExpiresAt < now)
            return CreateInvalidResult(coupon, "Mã giảm giá đã hết hạn.");

        if (coupon.UsageLimit.HasValue && coupon.UsedCount >= coupon.UsageLimit.Value)
            return CreateInvalidResult(coupon, "Mã giảm giá đã hết lượt sử dụng.");

        if (request.UserId.HasValue)
        {
            var hasUsed = await _couponRepository.HasUserUsedCouponAsync(
                request.UserId.Value, coupon.Id, cancellationToken);
            if (hasUsed)
                return CreateInvalidResult(coupon, "Bạn đã sử dụng mã giảm giá này.");
        }

        if (request.OrderAmount < coupon.MinOrderValue)
            return CreateInvalidResult(coupon, $"Giá trị đơn hàng tối thiểu là {coupon.MinOrderValue:N0}đ.");

        var discountAmount = CalculateDiscount(coupon, request.OrderAmount);

        return new ValidateCouponResultDto
        {
            CouponId = coupon.Id,
            Code = coupon.Code,
            IsValid = true,
            DiscountAmount = discountAmount,
            FinalAmount = request.OrderAmount - discountAmount
        };
    }

    private static decimal CalculateDiscount(Domain.Entities.Coupon coupon, decimal orderAmount)
    {
        decimal discount;

        if (coupon.Type == CouponType.Percent)
        {
            discount = orderAmount * (coupon.Value / 100);
        }
        else // Fixed
        {
            discount = coupon.Value;
        }

        if (coupon.MaxDiscount.HasValue && discount > coupon.MaxDiscount.Value)
            discount = coupon.MaxDiscount.Value;

        if (discount > orderAmount)
            discount = orderAmount;

        return Math.Round(discount, 0);
    }

    private static ValidateCouponResultDto CreateInvalidResult(Domain.Entities.Coupon? coupon, string errorMessage)
    {
        return new ValidateCouponResultDto
        {
            CouponId = coupon?.Id ?? Guid.Empty,
            Code = coupon?.Code ?? "",
            IsValid = false,
            ErrorMessage = errorMessage,
            DiscountAmount = 0,
            FinalAmount = 0
        };
    }
}
