using MediatR;

namespace InoxServer.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<int>
    {
        public int? ParentId { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public byte SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }
}
