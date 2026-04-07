using InoxServer.Application.Features.Banners.DTOs;
using MediatR;
using System.Collections.Generic;

namespace InoxServer.Application.Features.Banners.Queries.GetActiveBanners;

public record GetActiveBannersQuery : IRequest<List<BannerDto>>;
