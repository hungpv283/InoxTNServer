using InoxServer.Domain.Enums;
using MediatR;

namespace InoxServer.Application.Features.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommand : IRequest<bool>
{
    public Guid OrderId { get; set; }
    public OrderStatus NewStatus { get; set; }
    public Guid AdminUserId { get; set; }
    public string? Note { get; set; }
}
