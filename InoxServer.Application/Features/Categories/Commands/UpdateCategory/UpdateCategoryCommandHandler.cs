using InoxServer.Domain.Interfaces;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;

namespace InoxServer.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (category == null)
                return false;

            var slugExists = await _categoryRepository.ExistsBySlugAsync(request.Slug, cancellationToken);
            if (slugExists && category.Slug != request.Slug)
                throw new Exception($"Slug '{request.Slug}' already exists");

            if (request.ParentId.HasValue)
            {
                if (request.ParentId.Value == request.Id)
                    throw new Exception("Category cannot be its own parent");

                var parentExists = await _categoryRepository.ExistsAsync(request.ParentId.Value, cancellationToken);
                if (!parentExists)
                    throw new Exception("Parent category does not exist");
            }

            category.ParentId = request.ParentId;
            category.Name = request.Name;
            category.Slug = request.Slug;
            category.Description = request.Description;
            category.ImageUrl = request.ImageUrl;
            category.SortOrder = request.SortOrder;
            category.IsActive = request.IsActive;

            _categoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
