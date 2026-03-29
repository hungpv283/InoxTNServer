using InoxServer.Application.Features.Orders.DTOs;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.SharedKernel.Common;
using MediatR;

namespace InoxServer.Application.Features.Orders.Queries.GetAllOrders;

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, PagedResult<OrderSummaryDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetAllOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<PagedResult<OrderSummaryDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? 20 : request.PageSize;

        var paged = await _orderRepository.GetPagedAsync(
            page,
            pageSize,
            request.Status,
            request.OrderNumber,
            request.UserId,
            cancellationToken);

        var items = paged.Items.Select(o => new OrderSummaryDto
        {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            Status = o.Status,
            TotalAmount = o.TotalAmount,
            CreatedAt = o.CreatedAt,
            ItemCount = o.OrderItems.Count
        }).ToList();

        return new PagedResult<OrderSummaryDto>
        {
            Items = items,
            Page = paged.Page,
            PageSize = paged.PageSize,
            TotalCount = paged.TotalCount
        };
    }
}
