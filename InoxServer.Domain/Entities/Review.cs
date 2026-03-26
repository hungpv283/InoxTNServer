using InoxServer.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Entities
{
    public class Review : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public Guid? OrderId { get; set; }
        public byte Rating { get; set; }
        public string? Comment { get; set; }
        public bool IsApproved { get; set; } = false;
        public DateTime CreatedAt { get; set; }

        public Product Product { get; set; } = default!;
        public User User { get; set; } = default!;
        public Order? Order { get; set; }
    }
}
