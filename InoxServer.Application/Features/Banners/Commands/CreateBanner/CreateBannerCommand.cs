using MediatR;
using System;

namespace InoxServer.Application.Features.Banners.Commands.CreateBanner;

public class CreateBannerCommand : IRequest<Guid>
{
    public string? Title { get; set; }
    public string ImageUrl { get; set; } = default!;
    public string? LinkUrl { get; set; }
    public byte SortOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}
