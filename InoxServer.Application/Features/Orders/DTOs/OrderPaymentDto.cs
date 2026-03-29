using InoxServer.Domain.Enums;

namespace InoxServer.Application.Features.Orders.DTOs;

public class OrderPaymentDto
{
    public Guid Id { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }
    public decimal Amount { get; set; }
    public DateTime? PaidAt { get; set; }
}
