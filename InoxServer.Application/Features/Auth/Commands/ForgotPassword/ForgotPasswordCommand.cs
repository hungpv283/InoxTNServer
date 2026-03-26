using MediatR;

namespace InoxServer.Application.Features.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommand : IRequest
{
    public string Email { get; set; } = default!;
}
