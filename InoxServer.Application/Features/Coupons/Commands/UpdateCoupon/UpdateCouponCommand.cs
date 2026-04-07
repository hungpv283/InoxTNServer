using InoxServer.Domain.Enums;
using MediatR;
using System;

namespace InoxServer.Application.Features.Coupons.Commands.UpdateCoupon;

public class UpdateCouponCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Code { get; set; } = default!;
    public CouponType Type { get; set; }
    public decimal Value { get; set; }
    public decimal MinOrderValue { get; set; }
    public decimal? MaxDiscount { get; set; }
    public int? UsageLimit { get; set; }
    public bool IsActive { get; set; }
    public DateTime? StartsAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
