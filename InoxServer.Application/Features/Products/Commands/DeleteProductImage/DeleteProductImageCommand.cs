using MediatR;

namespace InoxServer.Application.Features.Products.Commands.DeleteProductImage;

public record DeleteProductImageCommand(Guid ProductId, Guid ImageId) : IRequest;
