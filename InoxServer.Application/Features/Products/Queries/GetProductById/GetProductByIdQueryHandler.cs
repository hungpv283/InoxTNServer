using InoxServer.Application.Features.Products.DTOs;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;

namespace InoxServer.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

            if (product == null)
                throw new DomainException(ProductErrors.NotFound);

            return new ProductDto
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                Name = product.Name,
                Slug = product.Slug,
                Price = product.Price,
                SalePrice = product.SalePrice,
                StockQty = product.StockQty,
                Sku = product.Sku,
                Material = product.Material,
                Dimensions = product.Dimensions,
                IsActive = product.IsActive,
                IsFeatured = product.IsFeatured,
                Images = product.ProductImages.Select(i => new ProductImageDto
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                    AltText = i.AltText,
                    IsPrimary = i.IsPrimary,
                    SortOrder = i.SortOrder
                }).ToList()
            };
        }
    }
}
