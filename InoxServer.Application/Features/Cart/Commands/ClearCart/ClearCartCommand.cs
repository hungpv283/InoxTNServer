using MediatR;

namespace InoxServer.Application.Features.Cart.Commands.ClearCart;

public class ClearCartCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
}

