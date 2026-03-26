namespace InoxServer.Application.Features.Cart.DTOs;

public class CheckoutCartResponseDto
{
    public Guid OrderId { get; set; }
    public string OrderNumber { get; set; } = default!;
    public decimal TotalAmount { get; set; }
}

