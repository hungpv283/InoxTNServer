using InoxServer.Application.Features.Wishlists.DTOs;
using MediatR;

namespace InoxServer.Application.Features.Wishlists.Queries.GetMyWishlist;

public record GetMyWishlistQuery : IRequest<WishlistDto>
{
    public Guid UserId { get; init; }
}
