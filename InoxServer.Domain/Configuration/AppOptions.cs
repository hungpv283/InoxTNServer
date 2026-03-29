namespace InoxServer.Domain.Configuration;

public class AppOptions
{
    public const string SectionName = "App";

    /// <summary>URL frontend dùng để ghép return/cancel URL PayOS khi client không gửi.</summary>
    public string FrontendUrl { get; set; } = "http://localhost:3000";
}
