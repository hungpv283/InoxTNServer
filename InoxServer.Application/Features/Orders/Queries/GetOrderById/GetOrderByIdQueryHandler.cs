using InoxServer.Application.Features.Orders.DTOs;
using InoxServer.Domain.Entities;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;

namespace InoxServer.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDetailDto>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderDetailDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
            throw new DomainException(OrderErrors.NotFound);

        if (!request.IsAdmin && order.UserId != request.RequestingUserId)
            throw new DomainException(OrderErrors.UnauthorizedAccess);

        return Map(order);
    }

    private static OrderDetailDto Map(Order order)
    {
        return new OrderDetailDto
        {
            Id = order.Id,
            UserId = order.UserId,
            OrderNumber = order.OrderNumber,
            Subtotal = order.Subtotal,
            ShippingFee = order.ShippingFee,
            DiscountAmount = order.DiscountAmount,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            ShippingName = order.ShippingName,
            ShippingPhone = order.ShippingPhone,
            ShippingAddress = order.ShippingAddress,
            ShippingProvince = order.ShippingProvince,
            Note = order.Note,
            CancelledReason = order.CancelledReason,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            Items = order.OrderItems.Select(i => new OrderItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Sku = i.Sku,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Subtotal = i.Subtotal
            }).ToList(),
            Payment = order.Payment is null
                ? null
                : new OrderPaymentDto
                {
                    Id = order.Payment.Id,
                    Method = order.Payment.Method,
                    Status = order.Payment.Status,
                    Amount = order.Payment.Amount,
                    PaidAt = order.Payment.PaidAt
                }
        };
    }
}
