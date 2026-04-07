using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading.Tasks;

namespace InoxServer.Application.Features.Banners.Commands.DeleteBanner;

public class DeleteBannerCommandHandler : IRequestHandler<DeleteBannerCommand, bool>
{
    private readonly IBannerRepository _bannerRepository;

    public DeleteBannerCommandHandler(IBannerRepository bannerRepository)
    {
        _bannerRepository = bannerRepository;
    }

    public async Task<bool> Handle(DeleteBannerCommand request, CancellationToken cancellationToken)
    {
        var banner = await _bannerRepository.GetByIdAsync(request.Id, cancellationToken);
        if (banner == null)
            throw new DomainException(BannerErrors.NotFound);

        _bannerRepository.Delete(banner);

        return true;
    }
}
