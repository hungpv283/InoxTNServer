using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Application.Features.Products.DTOs
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
    }
}
