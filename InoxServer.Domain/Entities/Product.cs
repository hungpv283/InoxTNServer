using InoxServer.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Entities
{
    public class Product : AuditableEntity
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public int StockQty { get; set; } = 0;
        public string Sku { get; set; } = default!;
        public string? Material { get; set; }
        public string? Dimensions { get; set; }
        public decimal? WeightKg { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;
        public decimal AvgRating { get; set; } = 0;
        public int ReviewCount { get; set; } = 0;
        public int ViewCount { get; set; } = 0;

        public Category Category { get; set; } = default!;

        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public ICollection<InventoryLog> InventoryLogs { get; set; } = new List<InventoryLog>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
    }
}
