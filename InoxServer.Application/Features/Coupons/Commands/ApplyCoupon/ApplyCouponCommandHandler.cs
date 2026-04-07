using InoxServer.Application.Features.Coupons.Commands.ValidateCoupon;
using InoxServer.Domain.Entities;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Infrastructure.Contexts;
using MediatR;
using System;
using System.Threading.Tasks;

namespace InoxServer.Application.Features.Coupons.Commands.ApplyCoupon;

public class ApplyCouponCommandHandler : IRequestHandler<ApplyCouponCommand, decimal>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IMediator _mediator;
    private readonly AppDbContext _context;

    public ApplyCouponCommandHandler(
        ICouponRepository couponRepository,
        IMediator mediator,
        AppDbContext context)
    {
        _couponRepository = couponRepository;
        _mediator = mediator;
        _context = context;
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

        await _context.CouponUsages.AddAsync(couponUsage, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return validationResult.DiscountAmount;
    }
}
