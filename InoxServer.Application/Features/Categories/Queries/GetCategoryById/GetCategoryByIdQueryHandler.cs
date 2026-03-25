using InoxServer.Application.Features.Categories.DTOs;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;

namespace InoxServer.Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (category == null)
                throw new DomainException(CategoryErrors.NotFound);

            return new CategoryDto
            {
                Id = category.Id,
                ParentId = category.ParentId,
                Name = category.Name,
                Slug = category.Slug,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                SortOrder = category.SortOrder,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt,
                Children = category.Children.Select(c => new CategoryDto
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
            };
        }
    }
}
