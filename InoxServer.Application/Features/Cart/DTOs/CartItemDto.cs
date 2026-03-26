namespace InoxServer.Application.Features.Cart.DTOs;

public class CartItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = default!;
    public string Sku { get; set; } = default!;

    public short Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal => UnitPrice * Quantity;

    public DateTime AddedAt { get; set; }
}

