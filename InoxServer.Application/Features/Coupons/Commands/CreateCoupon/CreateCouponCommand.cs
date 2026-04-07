using InoxServer.Domain.Enums;
using MediatR;
using System;

namespace InoxServer.Application.Features.Coupons.Commands.CreateCoupon;

public class CreateCouponCommand : IRequest<Guid>
{
    public string Code { get; set; } = default!;
    public CouponType Type { get; set; }
    public decimal Value { get; set; }
    public decimal MinOrderValue { get; set; } = 0;
    public decimal? MaxDiscount { get; set; }
    public int? UsageLimit { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? StartsAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
