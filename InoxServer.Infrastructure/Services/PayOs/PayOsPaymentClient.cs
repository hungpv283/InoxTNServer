using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using InoxServer.Domain.Configuration;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace InoxServer.Infrastructure.Services.PayOs;

public class PayOsPaymentClient : IPayOsPaymentClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly HttpClient _http;
    private readonly PayOsOptions _options;

    public PayOsPaymentClient(HttpClient http, IOptions<PayOsOptions> options)
    {
        _http = http;
        _options = options.Value;
    }

    public async Task<PayOsPaymentLinkResult> CreatePaymentLinkAsync(
        long orderCode,
        long amountVnd,
        string description,
        string returnUrl,
        string cancelUrl,
        CancellationToken cancellationToken = default)
    {
        if (!_options.Enabled)
            throw new DomainException(PaymentErrors.PayOsCreateFailed);

        if (string.IsNullOrWhiteSpace(_options.ClientId) ||
            string.IsNullOrWhiteSpace(_options.ApiKey) ||
            string.IsNullOrWhiteSpace(_options.ChecksumKey))
            throw new DomainException(PaymentErrors.PayOsCreateFailed);

        var desc = description.Length > 9 ? description[..9] : description;

        var signature = PayOsHmac.CreatePaymentRequestSignature(
            amountVnd,
            cancelUrl,
            desc,
            orderCode,
            returnUrl,
            _options.ChecksumKey);

        var payload = new
        {
            orderCode,
            amount = amountVnd,
            description = desc,
            returnUrl,
            cancelUrl,
            signature
        };

        using var response = await _http.PostAsJsonAsync("v2/payment-requests", payload, JsonOptions, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new DomainException(PaymentErrors.PayOsCreateFailed);

        var doc = JsonDocument.Parse(body);
        var root = doc.RootElement;
        var code = root.GetProperty("code").GetString();
        if (code != "00")
            throw new DomainException(PaymentErrors.PayOsCreateFailed);

        var data = root.GetProperty("data");
        return new PayOsPaymentLinkResult
        {
            OrderCode = data.GetProperty("orderCode").GetInt64(),
            PaymentLinkId = data.GetProperty("paymentLinkId").GetString()!,
            CheckoutUrl = data.GetProperty("checkoutUrl").GetString()!,
            QrCode = data.GetProperty("qrCode").GetString()!,
            RawResponseSignature = root.TryGetProperty("signature", out var sig) ? sig.GetString() : null
        };
    }
}
