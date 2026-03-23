using InoxServer.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Entities
{
    public class Banner : BaseEntity
    {
        public string? Title { get; set; }
        public string ImageUrl { get; set; } = default!;
        public string? LinkUrl { get; set; }
        public byte SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
    }
}
