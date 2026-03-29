using MediatR;

namespace InoxServer.Application.Features.Orders.Commands.CancelOrder;

public class CancelOrderCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
    public string? Reason { get; set; }
}
