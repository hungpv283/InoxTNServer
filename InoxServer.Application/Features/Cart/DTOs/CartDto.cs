using InoxServer.Application.Features.Cart.DTOs;

namespace InoxServer.Application.Features.Cart.DTOs;

public class CartDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<CartItemDto> Items { get; set; } = new();
}

