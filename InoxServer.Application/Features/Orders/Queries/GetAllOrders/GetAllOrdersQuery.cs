using InoxServer.Application.Features.Orders.DTOs;
using InoxServer.Domain.Enums;
using InoxServer.SharedKernel.Common;
using MediatR;

namespace InoxServer.Application.Features.Orders.Queries.GetAllOrders;

public class GetAllOrdersQuery : PaginationRequest, IRequest<PagedResult<OrderSummaryDto>>
{
    public OrderStatus? Status { get; set; }
    public string? OrderNumber { get; set; }
    public Guid? UserId { get; set; }
}
