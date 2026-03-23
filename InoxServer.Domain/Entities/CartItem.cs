using InoxServer.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public short Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public DateTime AddedAt { get; set; }

        public Cart Cart { get; set; } = default!;
        public Product Product { get; set; } = default!;
    }
}
