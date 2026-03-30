using InoxServer.Application.Features.Categories.DTOs;
using InoxServer.SharedKernel.Common;
using MediatR;

namespace InoxServer.Application.Features.Categories.Queries.GetCategories
{
    public class GetCategoriesQuery : PaginationRequest, IRequest<PagedResult<CategoryDto>>
    {
        public string? Keyword { get; set; }
        public Guid? ParentId { get; set; }
        public bool? IsActive { get; set; }
    }
}
