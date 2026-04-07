using InoxServer.Application.Features.Banners.DTOs;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace InoxServer.Application.Features.Banners.Queries.GetAllBanners;

public class GetAllBannersQueryHandler : IRequestHandler<GetAllBannersQuery, List<BannerDto>>
{
    private readonly IBannerRepository _bannerRepository;

    public GetAllBannersQueryHandler(IBannerRepository bannerRepository)
    {
        _bannerRepository = bannerRepository;
    }

    public async Task<List<BannerDto>> Handle(GetAllBannersQuery request, CancellationToken cancellationToken)
    {
        var banners = await _bannerRepository.GetAllAsync(cancellationToken);

        return banners
            .OrderBy(x => x.SortOrder)
            .Select(x => new BannerDto
            {
                Id = x.Id,
                Title = x.Title,
                ImageUrl = x.ImageUrl,
                LinkUrl = x.LinkUrl,
                SortOrder = x.SortOrder,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            })
            .ToList();
    }
}
