using InoxServer.Application.DTOs.Common;
using InoxServer.Domain.Enums;

namespace InoxServer.Application.Features.Orders.DTOs;

public class OrderSummaryDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = default!;
    public EnumDto<OrderStatus> Status { get; set; } = default!;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ItemCount { get; set; }
}
