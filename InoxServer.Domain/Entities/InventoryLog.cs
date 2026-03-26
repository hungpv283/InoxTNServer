using InoxServer.Domain.Common;
using InoxServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Entities
{
    public class InventoryLog : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Guid AdminId { get; set; }
        public InventoryLogType Type { get; set; }
        public int QuantityChange { get; set; }
        public int StockBefore { get; set; }
        public int StockAfter { get; set; }
        public string? ReferenceNo { get; set; }
        public string? SupplierName { get; set; }
        public decimal? UnitCost { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }

        public Product Product { get; set; } = default!;
        public User Admin { get; set; } = default!;
    }
}
