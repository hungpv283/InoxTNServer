using InoxServer.Application.Features.Banners.DTOs;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace InoxServer.Application.Features.Banners.Queries.GetActiveBanners;

public class GetActiveBannersQueryHandler : IRequestHandler<GetActiveBannersQuery, List<BannerDto>>
{
    private readonly IBannerRepository _bannerRepository;

    public GetActiveBannersQueryHandler(IBannerRepository bannerRepository)
    {
        _bannerRepository = bannerRepository;
    }

    public async Task<List<BannerDto>> Handle(GetActiveBannersQuery request, CancellationToken cancellationToken)
    {
        var banners = await _bannerRepository.GetActiveAsync(cancellationToken);

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
