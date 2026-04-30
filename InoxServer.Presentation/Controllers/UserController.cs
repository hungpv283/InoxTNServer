using InoxServer.Application.Features.Auth.Commands.UpdateProfile;
using InoxServer.Application.Features.Auth.Commands.UploadAvatar;
using InoxServer.Application.Features.Auth.Queries.GetProfile;
using InoxServer.Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InoxServer.Presentation.Controllers;

[ApiController]
[Route("api/v1/users")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _mediator.Send(new GetProfileQuery { UserId = userId });
        return Ok(result);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
    {
        command.UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("me/avatar")]
    public async Task<IActionResult> UploadAvatar(IFormFile file)
    {
        if (file is null || file.Length == 0)
            throw new DomainException(Error.BadRequest(
                DomainErrorCodes.General.UploadFailed,
                "Vui lòng chọn file ảnh."));

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType.ToLower()))
            throw new DomainException(Error.BadRequest(
                DomainErrorCodes.General.UploadFailed,
                "Chỉ chấp nhận file ảnh định dạng JPG, PNG hoặc WEBP."));

        // Giới hạn 5MB
        if (file.Length > 5 * 1024 * 1024)
            throw new DomainException(Error.BadRequest(
                DomainErrorCodes.General.UploadFailed,
                "Kích thước ảnh không được vượt quá 5MB."));

        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        using var stream = file.OpenReadStream();
        var avatarUrl = await _mediator.Send(new UploadAvatarCommand
        {
            UserId = userId,
            FileStream = stream,
            FileName = file.FileName
        });

        return Ok(new { avatarUrl });
    }
}
