using MediatR;

namespace InoxServer.Application.Features.Cart.Commands.AddToCart;

public class AddToCartCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public short Quantity { get; set; }
}

