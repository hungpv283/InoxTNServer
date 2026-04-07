using MediatR;
using System;

namespace InoxServer.Application.Features.Wishlists.Commands.AddToWishlist;

public class AddToWishlistCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
}
