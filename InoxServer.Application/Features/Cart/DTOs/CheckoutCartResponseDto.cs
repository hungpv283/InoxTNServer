using InoxServer.Application.DTOs.Common;
using InoxServer.Domain.Enums;

namespace InoxServer.Application.Features.Cart.DTOs;

public class CheckoutCartResponseDto
{
    public Guid OrderId { get; set; }
    public string OrderNumber { get; set; } = default!;
    public decimal TotalAmount { get; set; }

    public EnumDto<PaymentMethod> PaymentMethod { get; set; } = default!;

    public Guid? PaymentId { get; set; }

    /// <summary>Chỉ có khi thanh toán PayOS thành công tạo link.</summary>
    public string? PayOsCheckoutUrl { get; set; }

    public string? PayOsQrCode { get; set; }
}
