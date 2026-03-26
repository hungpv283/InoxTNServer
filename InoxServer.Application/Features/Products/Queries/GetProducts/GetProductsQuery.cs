using InoxServer.Application.Features.Products.DTOs;
using InoxServer.SharedKernel.Common;
using MediatR;

namespace InoxServer.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQuery : PaginationRequest, IRequest<PagedResult<ProductDto>>
    {
        public string? Keyword { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsActive { get; set; }
    }
}
