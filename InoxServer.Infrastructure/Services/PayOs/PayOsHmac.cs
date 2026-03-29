using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace InoxServer.Infrastructure.Services.PayOs;

internal static class PayOsHmac
{
    public static string CreatePaymentRequestSignature(
        long amount,
        string cancelUrl,
        string description,
        long orderCode,
        string returnUrl,
        string checksumKey)
    {
        var raw =
            $"amount={amount}&cancelUrl={cancelUrl}&description={description}&orderCode={orderCode}&returnUrl={returnUrl}";
        return HmacSha256Hex(raw, checksumKey);
    }

    public static string HmacSha256Hex(string data, string key)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    /// <summary>Webhook: ksort data, key=value& theo tài liệu PayOS (object phẳng).</summary>
    public static string CreateWebhookDataSignature(JsonElement dataObject, string checksumKey)
    {
        var pairs = new SortedDictionary<string, string>(StringComparer.Ordinal);
        foreach (var prop in dataObject.EnumerateObject())
            pairs[prop.Name] = FormatWebhookValue(prop.Value);

        var sb = new StringBuilder();
        var first = true;
        foreach (var kv in pairs)
        {
            if (!first) sb.Append('&');
            first = false;
            sb.Append(kv.Key).Append('=').Append(kv.Value);
        }

        return HmacSha256Hex(sb.ToString(), checksumKey);
    }

    private static string FormatWebhookValue(JsonElement el)
    {
        return el.ValueKind switch
        {
            JsonValueKind.String => el.GetString() ?? string.Empty,
            JsonValueKind.Number => el.GetRawText(),
            JsonValueKind.True => "true",
            JsonValueKind.False => "false",
            JsonValueKind.Null or JsonValueKind.Undefined => string.Empty,
            _ => el.GetRawText()
        };
    }
}
