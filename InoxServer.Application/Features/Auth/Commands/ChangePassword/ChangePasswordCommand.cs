using MediatR;

namespace InoxServer.Application.Features.Auth.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest
{
    public Guid UserId { get; set; }
    public string CurrentPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}
