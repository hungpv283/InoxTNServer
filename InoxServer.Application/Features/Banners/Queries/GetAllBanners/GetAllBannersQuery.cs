using InoxServer.Application.Features.Banners.DTOs;
using MediatR;
using System.Collections.Generic;

namespace InoxServer.Application.Features.Banners.Queries.GetAllBanners;

public record GetAllBannersQuery : IRequest<List<BannerDto>>;
