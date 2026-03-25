using MediatR;

namespace InoxServer.Application.Features.Auth.Commands.ResendVerifyEmail;

public class ResendVerifyEmailCommand : IRequest<bool>
{
    public string Email { get; set; } = default!;
}
