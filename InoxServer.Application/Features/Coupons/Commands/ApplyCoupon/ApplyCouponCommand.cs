using MediatR;
using System;

namespace InoxServer.Application.Features.Coupons.Commands.ApplyCoupon;

public class ApplyCouponCommand : IRequest<decimal>
{
    public string Code { get; set; } = default!;
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
    public decimal OrderAmount { get; set; }
}
