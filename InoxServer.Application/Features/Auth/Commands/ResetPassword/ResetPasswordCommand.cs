using MediatR;

namespace InoxServer.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommand : IRequest
{
    public string Token { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}
