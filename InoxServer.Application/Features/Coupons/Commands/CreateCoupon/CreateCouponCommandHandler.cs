using InoxServer.Domain.Entities;
using InoxServer.Domain.Enums;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading.Tasks;

namespace InoxServer.Application.Features.Coupons.Commands.CreateCoupon;

public class CreateCouponCommandHandler : IRequestHandler<CreateCouponCommand, Guid>
{
    private readonly ICouponRepository _couponRepository;

    public CreateCouponCommandHandler(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository;
    }

    public async Task<Guid> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        var normalizedCode = request.Code.ToUpper().Trim();

        if (await _couponRepository.ExistsByCodeAsync(normalizedCode, cancellationToken))
            throw new DomainException(CouponErrors.CodeAlreadyExists);

        var coupon = new Coupon
        {
            Id = Guid.NewGuid(),
            Code = normalizedCode,
            Type = request.Type,
            Value = request.Value,
            MinOrderValue = request.MinOrderValue,
            MaxDiscount = request.MaxDiscount,
            UsageLimit = request.UsageLimit,
            IsActive = request.IsActive,
            StartsAt = request.StartsAt,
            ExpiresAt = request.ExpiresAt,
            CreatedAt = DateTime.UtcNow
        };

        await _couponRepository.AddAsync(coupon, cancellationToken);

        return coupon.Id;
    }
}
