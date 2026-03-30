using InoxServer.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        public Guid ProductId { get; set; }
        public string ImageUrl { get; set; } = default!;
        public string? AltText { get; set; }
        public bool IsPrimary { get; set; } = false;
        public byte SortOrder { get; set; } = 0;

        public Product Product { get; set; } = default!;
    }
}
