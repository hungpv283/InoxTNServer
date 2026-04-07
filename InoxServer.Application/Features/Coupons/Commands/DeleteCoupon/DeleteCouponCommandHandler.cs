using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading.Tasks;

namespace InoxServer.Application.Features.Coupons.Commands.DeleteCoupon;

public class DeleteCouponCommandHandler : IRequestHandler<DeleteCouponCommand, bool>
{
    private readonly ICouponRepository _couponRepository;

    public DeleteCouponCommandHandler(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository;
    }

    public async Task<bool> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = await _couponRepository.GetByIdAsync(request.Id, cancellationToken);
        if (coupon == null)
            throw new DomainException(CouponErrors.NotFound);

        _couponRepository.Delete(coupon);

        return true;
    }
}
