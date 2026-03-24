using InoxServer.Application.Features.Products.DTOs;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.SharedKernel.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResult<ProductDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<PagedResult<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page <= 0 ? 1 : request.Page;
            var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            var pagedProducts = await _productRepository.GetPagedAsync(
                page,
                pageSize,
                request.Keyword,
                request.CategoryId,
                request.MinPrice,
                request.MaxPrice,
                request.IsActive,
                cancellationToken);

            var items = pagedProducts.Items.Select(x => new ProductDto
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

            return new PagedResult<ProductDto>
            {
                Items = items,
                Page = pagedProducts.Page,
                PageSize = pagedProducts.PageSize,
                TotalCount = pagedProducts.TotalCount
            };
        }
    }
}
