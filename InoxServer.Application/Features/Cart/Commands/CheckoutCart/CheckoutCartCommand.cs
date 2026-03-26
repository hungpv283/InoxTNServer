using InoxServer.Application.Features.Cart.DTOs;
using MediatR;

namespace InoxServer.Application.Features.Cart.Commands.CheckoutCart;

public class CheckoutCartCommand : IRequest<CheckoutCartResponseDto>
{
    public Guid UserId { get; set; }

    public decimal ShippingFee { get; set; } = 0;

    public string ShippingName { get; set; } = default!;
    public string ShippingPhone { get; set; } = default!;
    public string ShippingAddress { get; set; } = default!;

    public string? ShippingProvince { get; set; }
    public string? Note { get; set; }
}

