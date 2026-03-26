using MediatR;

namespace InoxServer.Application.Features.Cart.Commands.AdjustCartItemQuantity;

public class AdjustCartItemQuantityCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public short Delta { get; set; } // +1 / -1 (hoặc số khác)
}

