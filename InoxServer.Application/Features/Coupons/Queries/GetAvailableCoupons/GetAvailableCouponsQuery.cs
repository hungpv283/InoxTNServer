using InoxServer.Application.Features.Coupons.DTOs;
using MediatR;
using System.Collections.Generic;

namespace InoxServer.Application.Features.Coupons.Queries.GetAvailableCoupons;

public record GetAvailableCouponsQuery : IRequest<List<CouponDto>>;
