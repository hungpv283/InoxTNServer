using InoxServer.Application.Features.Wishlists.DTOs;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;
using System.Linq;

namespace InoxServer.Application.Features.Wishlists.Queries.GetMyWishlist;

public class GetMyWishlistQueryHandler : IRequestHandler<GetMyWishlistQuery, WishlistDto>
{
    private readonly IWishlistRepository _wishlistRepository;

    public GetMyWishlistQueryHandler(IWishlistRepository wishlistRepository)
    {
        _wishlistRepository = wishlistRepository;
    }

    public async Task<WishlistDto> Handle(GetMyWishlistQuery request, CancellationToken cancellationToken)
    {
        var wishlistItems = await _wishlistRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        var items = wishlistItems.Select(w => new WishlistItemDto
        {
            Id = w.Id,
            ProductId = w.ProductId,
            ProductName = w.Product?.Name ?? "Sản phẩm không tồn tại",
            ProductSlug = w.Product?.Slug ?? "",
            ProductImage = w.Product?.ProductImages?.FirstOrDefault()?.ImageUrl,
            Price = w.Product?.Price ?? 0,
            SalePrice = w.Product?.SalePrice,
            StockQty = w.Product?.StockQty ?? 0,
            IsActive = w.Product?.IsActive ?? false,
            AddedAt = w.AddedAt
        }).ToList();

        return new WishlistDto
        {
            TotalItems = items.Count,
            Items = items
        };
    }
}
