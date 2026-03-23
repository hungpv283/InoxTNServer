using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Application.Features.Products.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public int StockQty { get; set; }
        public string Sku { get; set; } = default!;
        public string? Material { get; set; }
        public string? Dimensions { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
    }
}
