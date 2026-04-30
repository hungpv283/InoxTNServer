using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public int StockQty { get; set; }
        public string Sku { get; set; } = default!;
        public string? Material { get; set; }
        public string? Dimensions { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;
        /// <summary>Danh sách URL ảnh. Ảnh đầu tiên tự động là primary.</summary>
        public List<string> ImageUrls { get; set; } = new();
    }
}
