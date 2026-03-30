namespace InoxServer.Application.Features.Auth.DTOs;

public class ProfileDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? AvatarUrl { get; set; }
    public string Role { get; set; } = default!;
    public bool IsEmailVerified { get; set; }
    public DateTime CreatedAt { get; set; }
}
