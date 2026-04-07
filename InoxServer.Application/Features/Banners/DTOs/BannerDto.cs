using System;

namespace InoxServer.Application.Features.Banners.DTOs;

public class BannerDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string ImageUrl { get; set; } = default!;
    public string? LinkUrl { get; set; }
    public byte SortOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
