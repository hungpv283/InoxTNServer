namespace InoxServer.Domain.Interfaces.Services;

/// <summary>Tạo link thanh toán PayOS (POST /v2/payment-requests).</summary>
public interface IPayOsPaymentClient
{
    Task<PayOsPaymentLinkResult> CreatePaymentLinkAsync(
        long orderCode,
        long amountVnd,
        string description,
        string returnUrl,
        string cancelUrl,
        CancellationToken cancellationToken = default);
}

public sealed class PayOsPaymentLinkResult
{
    public long OrderCode { get; init; }
    public string PaymentLinkId { get; init; } = default!;
    public string CheckoutUrl { get; init; } = default!;
    public string QrCode { get; init; } = default!;
    public string? RawResponseSignature { get; init; }
}
