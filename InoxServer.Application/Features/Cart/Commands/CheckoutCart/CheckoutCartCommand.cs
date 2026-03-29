using InoxServer.Application.Features.Cart.DTOs;
using InoxServer.Domain.Enums;
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

    /// <summary>COD: thanh toán khi nhận. PayOS: tạo link thanh toán ngay sau khi tạo đơn.</summary>
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cod;

    /// <summary>Tuỳ chọn — nếu null sẽ dùng App:FrontendUrl (PayOS).</summary>
    public string? PayOsReturnUrl { get; set; }

    /// <summary>Tuỳ chọn — nếu null sẽ dùng App:FrontendUrl (PayOS).</summary>
    public string? PayOsCancelUrl { get; set; }
}

