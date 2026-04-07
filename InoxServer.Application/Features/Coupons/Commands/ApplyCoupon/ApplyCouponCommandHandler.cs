using InoxServer.Application.Features.Coupons.Commands.ValidateCoupon;
using InoxServer.Domain.Entities;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;
using System;
using System.Threading.Tasks;

namespace InoxServer.Application.Features.Coupons.Commands.ApplyCoupon;

public class ApplyCouponCommandHandler : IRequestHandler<ApplyCouponCommand, decimal>
{
    private readonly ICouponRepository _couponRepository;
    private readonly ICouponUsageRepository _couponUsageRepository;
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public ApplyCouponCommandHandler(
        ICouponRepository couponRepository,
        ICouponUsageRepository couponUsageRepository,
        IMediator mediator,
        IUnitOfWork unitOfWork)
    {
        _couponRepository = couponRepository;
        _couponUsageRepository = couponUsageRepository;
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<decimal> Handle(ApplyCouponCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate coupon (tái sử dụng logic từ ValidateCouponCommandHandler)
        var validationResult = await _mediator.Send(new ValidateCouponCommand
        {
            Code = request.Code,
            UserId = request.UserId,
            OrderAmount = request.OrderAmount
        }, cancellationToken);

        if (!validationResult.IsValid)
            throw new InvalidOperationException(validationResult.ErrorMessage ?? "Coupon không hợp lệ.");

        // 2. Lấy coupon và cập nhật UsedCount
        var coupon = await _couponRepository.GetByCodeAsync(request.Code.ToUpper().Trim(), cancellationToken);
        if (coupon == null)
            throw new InvalidOperationException("Coupon không tồn tại.");

        coupon.UsedCount++;
        _couponRepository.Update(coupon);

        // 3. Tạo CouponUsage record
        var couponUsage = new CouponUsage
        {
            Id = Guid.NewGuid(),
            CouponId = coupon.Id,
            UserId = request.UserId,
            OrderId = request.OrderId,
            DiscountApplied = validationResult.DiscountAmount,
            UsedAt = DateTime.UtcNow
        };

        await _couponUsageRepository.AddAsync(couponUsage, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return validationResult.DiscountAmount;
    }
}
