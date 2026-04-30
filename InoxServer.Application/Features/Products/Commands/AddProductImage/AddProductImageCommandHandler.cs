using InoxServer.Application.Features.Products.DTOs;
using InoxServer.Domain.Entities;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;

namespace InoxServer.Application.Features.Products.Commands.AddProductImage;

public class AddProductImageCommandHandler : IRequestHandler<AddProductImageCommand, ProductImageDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductImageRepository _imageRepository;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IUnitOfWork _unitOfWork;

    public AddProductImageCommandHandler(
        IProductRepository productRepository,
        IProductImageRepository imageRepository,
        ICloudinaryService cloudinaryService,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _imageRepository = imageRepository;
        _cloudinaryService = cloudinaryService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductImageDto> Handle(AddProductImageCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            throw new DomainException(ProductErrors.NotFound);

        var imageUrl = await _cloudinaryService.UploadImageAsync(
            request.FileStream,
            request.FileName,
            folder: "products",
            cancellationToken);

        // Nếu đánh dấu primary, bỏ primary của ảnh cũ
        if (request.IsPrimary)
        {
            foreach (var img in product.ProductImages)
                img.IsPrimary = false;
        }

        var sortOrder = product.ProductImages.Any()
            ? (byte)(product.ProductImages.Max(i => i.SortOrder) + 1)
            : (byte)0;

        var image = new ProductImage
        {
            ProductId = request.ProductId,
            ImageUrl = imageUrl,
            IsPrimary = request.IsPrimary || !product.ProductImages.Any(),
            SortOrder = sortOrder
        };

        await _imageRepository.AddAsync(image, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ProductImageDto
        {
            Id = image.Id,
            ImageUrl = image.ImageUrl,
            AltText = image.AltText,
            IsPrimary = image.IsPrimary,
            SortOrder = image.SortOrder
        };
    }
}
