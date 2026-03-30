using InoxServer.Application.Features.Auth.DTOs;
using MediatR;

namespace InoxServer.Application.Features.Auth.Queries.GetProfile;

public class GetProfileQuery : IRequest<ProfileDto>
{
    public Guid UserId { get; set; }
}
