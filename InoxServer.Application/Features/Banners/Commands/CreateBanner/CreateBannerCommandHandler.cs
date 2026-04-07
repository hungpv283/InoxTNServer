using InoxServer.Domain.Entities;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading.Tasks;

namespace InoxServer.Application.Features.Banners.Commands.CreateBanner;

public class CreateBannerCommandHandler : IRequestHandler<CreateBannerCommand, Guid>
{
    private readonly IBannerRepository _bannerRepository;

    public CreateBannerCommandHandler(IBannerRepository bannerRepository)
    {
        _bannerRepository = bannerRepository;
    }

    public async Task<Guid> Handle(CreateBannerCommand request, CancellationToken cancellationToken)
    {
        var banner = new Banner
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            ImageUrl = request.ImageUrl,
            LinkUrl = request.LinkUrl,
            SortOrder = request.SortOrder,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        await _bannerRepository.AddAsync(banner, cancellationToken);

        return banner.Id;
    }
}
