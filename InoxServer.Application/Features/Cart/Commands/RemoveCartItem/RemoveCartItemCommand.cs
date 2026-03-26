using MediatR;

namespace InoxServer.Application.Features.Cart.Commands.RemoveCartItem;

public class RemoveCartItemCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
}

