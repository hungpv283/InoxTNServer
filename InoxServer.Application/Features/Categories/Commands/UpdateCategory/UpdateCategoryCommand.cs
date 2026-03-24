using MediatR;

namespace InoxServer.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public byte SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
