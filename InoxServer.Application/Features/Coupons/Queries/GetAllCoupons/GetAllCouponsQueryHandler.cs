using InoxServer.Application.Features.Coupons.DTOs;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace InoxServer.Application.Features.Coupons.Queries.GetAllCoupons;

public class GetAllCouponsQueryHandler : IRequestHandler<GetAllCouponsQuery, List<CouponDto>>
{
    private readonly ICouponRepository _couponRepository;

    public GetAllCouponsQueryHandler(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository;
    }

    public async Task<List<CouponDto>> Handle(GetAllCouponsQuery request, CancellationToken cancellationToken)
    {
        var coupons = await _couponRepository.GetAllAsync(cancellationToken);

        return coupons.Select(c => new CouponDto
        {
            Id = c.Id,
            Code = c.Code,
            Type = c.Type,
            Value = c.Value,
            MinOrderValue = c.MinOrderValue,
            MaxDiscount = c.MaxDiscount,
            UsageLimit = c.UsageLimit,
            UsedCount = c.UsedCount,
            IsActive = c.IsActive,
            StartsAt = c.StartsAt,
            ExpiresAt = c.ExpiresAt,
            RemainingUses = c.UsageLimit.HasValue ? c.UsageLimit.Value - c.UsedCount : int.MaxValue
        }).ToList();
    }
}
