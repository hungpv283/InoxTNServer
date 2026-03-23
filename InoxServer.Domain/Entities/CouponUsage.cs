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
        public int CouponId { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public decimal DiscountApplied { get; set; }
        public DateTime UsedAt { get; set; }

        public Coupon Coupon { get; set; } = default!;
        public User User { get; set; } = default!;
        public Order Order { get; set; } = default!;
    }
}
