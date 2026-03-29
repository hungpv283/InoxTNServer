namespace InoxServer.Domain.Configuration;

public class PayOsOptions
{
    public const string SectionName = "PayOs";

    /// <summary>Client ID từ kênh PayOS (header x-client-id).</summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>API Key (header x-api-key).</summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>Checksum key để ký request và kiểm tra webhook.</summary>
    public string ChecksumKey { get; set; } = string.Empty;

    public string BaseUrl { get; set; } = "https://api-merchant.payos.vn";

    /// <summary>Bật false để tắt gọi API PayOS (dev/test không có key).</summary>
    public bool Enabled { get; set; } = true;
}
