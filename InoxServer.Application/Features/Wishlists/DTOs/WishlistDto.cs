using System.Collections.Generic;

namespace InoxServer.Application.Features.Wishlists.DTOs;

public class WishlistDto
{
    public int TotalItems { get; set; }
    public List<WishlistItemDto> Items { get; set; } = new();
}
