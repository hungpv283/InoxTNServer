using MediatR;
using System;

namespace InoxServer.Application.Features.Coupons.Commands.DeleteCoupon;

public class DeleteCouponCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
