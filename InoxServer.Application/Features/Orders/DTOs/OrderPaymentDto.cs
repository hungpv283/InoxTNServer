using InoxServer.Application.DTOs.Common;
using InoxServer.Domain.Enums;

namespace InoxServer.Application.Features.Orders.DTOs;

public class OrderPaymentDto
{
    public Guid Id { get; set; }
    public EnumDto<PaymentMethod> Method { get; set; } = default!;
    public EnumDto<PaymentStatus> Status { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTime? PaidAt { get; set; }
}
