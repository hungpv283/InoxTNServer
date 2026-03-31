using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Enums
{
    public enum OrderStatus
    {
        Unknown = 0,
        Pending = 1,
        Confirmed = 2,
        Processing = 3,
        Shipping = 4,
        Delivered = 5,
        Completed = 8,
        Cancelled = 6,
        Refunded = 7
    }
}
