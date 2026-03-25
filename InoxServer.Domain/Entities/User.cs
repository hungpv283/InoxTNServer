using InoxServer.Domain.Common;
using InoxServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Entities
{
    public class User : AuditableEntity
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public UserRole Role { get; set; } = UserRole.Customer;
        public bool IsActive { get; set; } = true;
        public DateTime? EmailVerifiedAt { get; set; }
        public string? EmailVerificationToken { get; set; }
        public DateTime? EmailVerificationTokenExpiresAt { get; set; }

        public Cart? Cart { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
        public ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();
        public ICollection<InventoryLog> InventoryLogs { get; set; } = new List<InventoryLog>();
        public ICollection<OrderStatusLog> OrderStatusLogs { get; set; } = new List<OrderStatusLog>();
    }
}
