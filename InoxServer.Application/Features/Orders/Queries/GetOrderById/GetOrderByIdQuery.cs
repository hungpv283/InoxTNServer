using InoxServer.Application.Features.Orders.DTOs;
using MediatR;

namespace InoxServer.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQuery : IRequest<OrderDetailDto>
{
    public Guid OrderId { get; set; }
    public Guid RequestingUserId { get; set; }
    public bool IsAdmin { get; set; }
}
