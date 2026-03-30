using InoxServer.Application.Features.Categories.DTOs;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.SharedKernel.Common;
using MediatR;

namespace InoxServer.Application.Features.Categories.Queries.GetCategories
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, PagedResult<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<PagedResult<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page <= 0 ? 1 : request.Page;
            var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            var paged = await _categoryRepository.GetPagedAsync(
                page,
                pageSize,
                request.Keyword,
                request.ParentId,
                request.IsActive,
                cancellationToken);

            var items = paged.Items.Select(x => new CategoryDto
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                SortOrder = x.SortOrder,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                Children = x.Children.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    ParentId = c.ParentId,
                    Name = c.Name,
                    Slug = c.Slug,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                    SortOrder = c.SortOrder,
                    IsActive = c.IsActive,
                    CreatedAt = c.CreatedAt
                }).ToList()
            }).ToList();

            return new PagedResult<CategoryDto>
            {
                Items = items,
                Page = paged.Page,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount
            };
        }
    }
}
