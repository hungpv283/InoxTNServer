using MediatR;

namespace InoxServer.Application.Features.Auth.Commands.VerifyEmail;

public class VerifyEmailCommand : IRequest<bool>
{
    public string Token { get; set; } = default!;
}
