using InoxServer.Domain.Common;
using InoxServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Entities
{
    public class Coupon : BaseEntity
    {
        public string Code { get; set; } = default!;
        public CouponType Type { get; set; }
        public decimal Value { get; set; }
        public decimal MinOrderValue { get; set; } = 0;
        public decimal? MaxDiscount { get; set; }
        public int? UsageLimit { get; set; }
        public int UsedCount { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public DateTime? StartsAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();
    }
}
