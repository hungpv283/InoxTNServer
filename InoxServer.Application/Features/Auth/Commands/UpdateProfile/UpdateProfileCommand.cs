using InoxServer.Application.Features.Auth.DTOs;
using MediatR;

namespace InoxServer.Application.Features.Auth.Commands.UpdateProfile;

public class UpdateProfileCommand : IRequest<ProfileDto>
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = default!;
    public string? Phone { get; set; }
    public string? Address { get; set; }
}
