namespace InoxServer.Application.Features.Coupons.DTOs;

public class ValidateCouponResultDto
{
    public Guid CouponId { get; set; }
    public string Code { get; set; } = default!;
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }
}
