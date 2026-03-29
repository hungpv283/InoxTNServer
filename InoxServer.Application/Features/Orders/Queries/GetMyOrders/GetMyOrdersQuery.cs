using InoxServer.Application.Features.Orders.DTOs;
using MediatR;

namespace InoxServer.Application.Features.Orders.Queries.GetMyOrders;

public class GetMyOrdersQuery : IRequest<List<OrderSummaryDto>>
{
    public Guid UserId { get; set; }
}
