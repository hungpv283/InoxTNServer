using InoxServer.Domain.Entities;

namespace InoxServer.Domain.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
