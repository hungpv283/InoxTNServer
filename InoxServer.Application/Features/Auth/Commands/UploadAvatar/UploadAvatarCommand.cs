using MediatR;

namespace InoxServer.Application.Features.Auth.Commands.UploadAvatar;

public class UploadAvatarCommand : IRequest<string>
{
    public Guid UserId { get; set; }

    /// <summary>Stream của file ảnh từ IFormFile (được truyền từ Controller)</summary>
    public Stream FileStream { get; set; } = default!;

    /// <summary>Tên file gốc (vd: "avatar.jpg")</summary>
    public string FileName { get; set; } = default!;
}
