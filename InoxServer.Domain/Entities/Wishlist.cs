using InoxServer.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Entities
{
    public class Wishlist : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public DateTime AddedAt { get; set; }

        public User User { get; set; } = default!;
        public Product Product { get; set; } = default!;
    }
}
