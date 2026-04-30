using InoxServer.Application.Features.Products.DTOs;
using MediatR;

namespace InoxServer.Application.Features.Products.Commands.AddProductImage;

public class AddProductImageCommand : IRequest<ProductImageDto>
{
    public Guid ProductId { get; set; }
    public Stream FileStream { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public bool IsPrimary { get; set; } = false;
}
