using System;
using System.Collections.Generic;

namespace InoxServer.Application.Features.Categories.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public byte SortOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CategoryDto> Children { get; set; } = new();
    }
}
