using InoxServer.Application.Features.Coupons.DTOs;
using MediatR;

namespace InoxServer.Application.Features.Coupons.Commands.ValidateCoupon;

public class ValidateCouponCommand : IRequest<ValidateCouponResultDto>
{
    public string Code { get; set; } = default!;
    public Guid? UserId { get; set; }
    public decimal OrderAmount { get; set; }
}
