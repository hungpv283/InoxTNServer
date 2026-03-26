using MediatR;

namespace InoxServer.Application.Features.Cart.Commands.UpdateCartItemQuantity;

public class UpdateCartItemQuantityCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public short Quantity { get; set; }
}

