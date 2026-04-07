using InoxServer.Domain.Enums;

namespace InoxServer.Application.Features.Coupons.DTOs;

public class CouponDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = default!;
    public CouponType Type { get; set; }
    public decimal Value { get; set; }
    public decimal MinOrderValue { get; set; }
    public decimal? MaxDiscount { get; set; }
    public int? UsageLimit { get; set; }
    public int UsedCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime? StartsAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsAvailable { get; set; }
    public int RemainingUses { get; set; }
}
