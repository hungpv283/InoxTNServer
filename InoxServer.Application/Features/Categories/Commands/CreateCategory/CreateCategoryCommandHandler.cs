using InoxServer.Domain.Entities;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Services;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;

namespace InoxServer.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var slugExists = await _categoryRepository.ExistsBySlugAsync(request.Slug, cancellationToken);
            if (slugExists)
                throw new DomainException(CategoryErrors.SlugAlreadyExists);

            if (request.ParentId.HasValue)
            {
                var parentExists = await _categoryRepository.ExistsAsync(request.ParentId.Value, cancellationToken);
                if (!parentExists)
                    throw new DomainException(CategoryErrors.ParentNotFound);
            }

            var category = new Category
            {
                ParentId = request.ParentId,
                Name = request.Name,
                Slug = request.Slug,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                SortOrder = request.SortOrder,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            await _categoryRepository.AddAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return category.Id;
        }
    }
}
