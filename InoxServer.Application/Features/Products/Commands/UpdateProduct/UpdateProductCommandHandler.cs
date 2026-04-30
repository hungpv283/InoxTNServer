using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;

namespace InoxServer.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

            if (product == null)
                throw new DomainException(ProductErrors.NotFound);

            product.CategoryId = request.CategoryId;
            product.Name = request.Name;
            product.Slug = request.Slug;
            product.Description = request.Description;
            product.Price = request.Price;
            product.SalePrice = request.SalePrice;
            product.StockQty = request.StockQty;
            product.Sku = request.Sku;
            product.Material = request.Material;
            product.Dimensions = request.Dimensions;
            product.IsActive = request.IsActive;
            product.IsFeatured = request.IsFeatured;
            product.UpdatedAt = DateTime.UtcNow;

            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
