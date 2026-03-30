using InoxServer.Domain.Common;
using InoxServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Entities
{
    public class Order : AuditableEntity
    {
        public Guid UserId { get; set; }
        public string OrderNumber { get; set; } = default!;
        public decimal Subtotal { get; set; }
        public decimal ShippingFee { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string ShippingName { get; set; } = default!;
        public string ShippingPhone { get; set; } = default!;
        public string ShippingAddress { get; set; } = default!;
        public string? ShippingProvince { get; set; }
        public string? Note { get; set; }
        public string? CancelledReason { get; set; }

        public User User { get; set; } = default!;
        public Payment? Payment { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<OrderStatusLog> OrderStatusLogs { get; set; } = new List<OrderStatusLog>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();
    }
}
