using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;

namespace InoxServer.Application.Features.Products.Commands.DeleteProductImage;

public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommand>
{
    private readonly IProductImageRepository _imageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductImageCommandHandler(
        IProductImageRepository imageRepository,
        IUnitOfWork unitOfWork)
    {
        _imageRepository = imageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
    {
        var image = await _imageRepository.GetByIdAsync(request.ImageId, cancellationToken);

        if (image is null || image.ProductId != request.ProductId)
            throw new DomainException(Error.NotFound("Product.ImageNotFound", "Không tìm thấy ảnh."));

        _imageRepository.Delete(image);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
