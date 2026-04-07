using MediatR;
using System;

namespace InoxServer.Application.Features.Banners.Commands.UpdateBanner;

public class UpdateBannerCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string ImageUrl { get; set; } = default!;
    public string? LinkUrl { get; set; }
    public byte SortOrder { get; set; }
    public bool IsActive { get; set; }
}
