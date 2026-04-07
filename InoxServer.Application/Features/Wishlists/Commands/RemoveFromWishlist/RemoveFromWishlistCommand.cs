using MediatR;
using System;

namespace InoxServer.Application.Features.Wishlists.Commands.RemoveFromWishlist;

public class RemoveFromWishlistCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
}
