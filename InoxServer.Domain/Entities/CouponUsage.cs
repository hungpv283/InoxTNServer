using InoxServer.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Entities
{
    public class CouponUsage : BaseEntity
    {
        public Guid CouponId { get; set; }
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public decimal DiscountApplied { get; set; }
        public DateTime UsedAt { get; set; }

        public Coupon Coupon { get; set; } = default!;
        public User User { get; set; } = default!;
        public Order Order { get; set; } = default!;
    }
}
