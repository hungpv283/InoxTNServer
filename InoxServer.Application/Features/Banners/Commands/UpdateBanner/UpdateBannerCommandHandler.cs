using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading.Tasks;

namespace InoxServer.Application.Features.Banners.Commands.UpdateBanner;

public class UpdateBannerCommandHandler : IRequestHandler<UpdateBannerCommand, bool>
{
    private readonly IBannerRepository _bannerRepository;

    public UpdateBannerCommandHandler(IBannerRepository bannerRepository)
    {
        _bannerRepository = bannerRepository;
    }

    public async Task<bool> Handle(UpdateBannerCommand request, CancellationToken cancellationToken)
    {
        var banner = await _bannerRepository.GetByIdAsync(request.Id, cancellationToken);
        if (banner == null)
            throw new DomainException(BannerErrors.NotFound);

        banner.Title = request.Title;
        banner.ImageUrl = request.ImageUrl;
        banner.LinkUrl = request.LinkUrl;
        banner.SortOrder = request.SortOrder;
        banner.IsActive = request.IsActive;

        _bannerRepository.Update(banner);

        return true;
    }
}
