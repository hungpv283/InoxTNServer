using InoxServer.Application.Features.Coupons.DTOs;
using MediatR;
using System.Collections.Generic;

namespace InoxServer.Application.Features.Coupons.Queries.GetAllCoupons;

public record GetAllCouponsQuery : IRequest<List<CouponDto>>;
