using System.Text.Json;

namespace InoxServer.Domain.Interfaces.Services;

public interface IPayOsWebhookSignatureVerifier
{
    /// <summary>Kiểm tra signature của object <c>data</c> trong payload webhook.</summary>
    bool IsValid(JsonElement data, string signatureFromPayload);
}
