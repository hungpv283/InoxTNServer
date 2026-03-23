using InoxServer.Application.Features.Products.DTOs;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync(cancellationToken);

            return products.Select(x => new ProductDto
            {
                Id = x.Id,
                CategoryId = x.CategoryId,
                Name = x.Name,
                Slug = x.Slug,
                Price = x.Price,
                SalePrice = x.SalePrice,
                StockQty = x.StockQty,
                Sku = x.Sku,
                Material = x.Material,
                Dimensions = x.Dimensions,
                IsActive = x.IsActive,
                IsFeatured = x.IsFeatured
            }).ToList();
        }
    }
}
