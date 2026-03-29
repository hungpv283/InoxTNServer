using InoxServer.Domain.Enums;

namespace InoxServer.Application.Features.Orders.DTOs;

public class OrderDetailDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string OrderNumber { get; set; } = default!;

    public decimal Subtotal { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }

    public OrderStatus Status { get; set; }

    public string ShippingName { get; set; } = default!;
    public string ShippingPhone { get; set; } = default!;
    public string ShippingAddress { get; set; } = default!;
    public string? ShippingProvince { get; set; }
    public string? Note { get; set; }
    public string? CancelledReason { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<OrderItemDto> Items { get; set; } = new();
    public OrderPaymentDto? Payment { get; set; }
}
