using System;

namespace InoxServer.Application.Features.Wishlists.DTOs;

public class WishlistItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public string ProductSlug { get; set; } = default!;
    public string? ProductImage { get; set; }
    public decimal Price { get; set; }
    public decimal? SalePrice { get; set; }
    public int StockQty { get; set; }
    public bool IsActive { get; set; }
    public DateTime AddedAt { get; set; }
}
