using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading.Tasks;

namespace InoxServer.Application.Features.Coupons.Commands.UpdateCoupon;

public class UpdateCouponCommandHandler : IRequestHandler<UpdateCouponCommand, bool>
{
    private readonly ICouponRepository _couponRepository;

    public UpdateCouponCommandHandler(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository;
    }

    public async Task<bool> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = await _couponRepository.GetByIdAsync(request.Id, cancellationToken);
        if (coupon == null)
            throw new DomainException(CouponErrors.NotFound);

        var normalizedCode = request.Code.ToUpper().Trim();

        // Kiểm tra code trùng (nếu đổi code)
        if (coupon.Code != normalizedCode)
        {
            var existingCoupon = await _couponRepository.GetByCodeAsync(normalizedCode, cancellationToken);
            if (existingCoupon != null && existingCoupon.Id != request.Id)
                throw new DomainException(CouponErrors.CodeAlreadyExists);
        }

        coupon.Code = normalizedCode;
        coupon.Type = request.Type;
        coupon.Value = request.Value;
        coupon.MinOrderValue = request.MinOrderValue;
        coupon.MaxDiscount = request.MaxDiscount;
        coupon.UsageLimit = request.UsageLimit;
        coupon.IsActive = request.IsActive;
        coupon.StartsAt = request.StartsAt;
        coupon.ExpiresAt = request.ExpiresAt;

        _couponRepository.Update(coupon);

        return true;
    }
}
