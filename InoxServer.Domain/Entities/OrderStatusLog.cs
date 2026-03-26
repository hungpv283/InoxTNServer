using InoxServer.Domain.Common;
using InoxServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Entities
{
    public class OrderStatusLog : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid? AdminId { get; set; }
        public OrderStatus StatusFrom { get; set; }
        public OrderStatus StatusTo { get; set; }
        public string? Note { get; set; }
        public DateTime ChangedAt { get; set; }

        public Order Order { get; set; } = default!;
        public User? Admin { get; set; }
    }
}
