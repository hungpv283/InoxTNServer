using MediatR;
using System;

namespace InoxServer.Application.Features.Banners.Commands.DeleteBanner;

public class DeleteBannerCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
