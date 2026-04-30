using InoxServer.Domain.Entities;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;

namespace InoxServer.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _imageRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(
            IProductRepository productRepository,
            IProductImageRepository imageRepository,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _imageRepository = imageRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var exists = await _categoryRepository.ExistsAsync(request.CategoryId, cancellationToken);
            if (!exists)
                throw new DomainException(ProductErrors.CategoryNotFound);

            var product = new Product
            {
                CategoryId = request.CategoryId,
                Name = request.Name,
                Slug = request.Slug,
                Description = request.Description,
                Price = request.Price,
                SalePrice = request.SalePrice,
                StockQty = request.StockQty,
                Sku = request.Sku,
                Material = request.Material,
                Dimensions = request.Dimensions,
                IsActive = request.IsActive,
                IsFeatured = request.IsFeatured,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _productRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Lưu ảnh từ URL nếu có
            for (byte i = 0; i < request.ImageUrls.Count; i++)
            {
                var url = request.ImageUrls[i].Trim();
                if (string.IsNullOrEmpty(url)) continue;

                await _imageRepository.AddAsync(new ProductImage
                {
                    ProductId = product.Id,
                    ImageUrl = url,
                    IsPrimary = i == 0,
                    SortOrder = i
                }, cancellationToken);
            }

            if (request.ImageUrls.Count > 0)
                await _unitOfWork.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
