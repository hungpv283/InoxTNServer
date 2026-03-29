using System.Text.Json;
using InoxServer.Domain.Configuration;
using InoxServer.Domain.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace InoxServer.Infrastructure.Services.PayOs;

public class PayOsWebhookSignatureVerifier : IPayOsWebhookSignatureVerifier
{
    private readonly PayOsOptions _options;

    public PayOsWebhookSignatureVerifier(IOptions<PayOsOptions> options)
    {
        _options = options.Value;
    }

    public bool IsValid(JsonElement data, string signatureFromPayload)
    {
        if (string.IsNullOrEmpty(signatureFromPayload) || string.IsNullOrEmpty(_options.ChecksumKey))
            return false;

        if (data.ValueKind != JsonValueKind.Object)
            return false;

        var computed = PayOsHmac.CreateWebhookDataSignature(data, _options.ChecksumKey);
        return string.Equals(computed, signatureFromPayload, StringComparison.OrdinalIgnoreCase);
    }
}
