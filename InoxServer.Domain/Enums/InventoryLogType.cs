using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Enums
{
    public enum InventoryLogType
    {
        Import = 1,
        Export = 2,
        Adjustment = 3,
        OrderDeduct = 4,
        OrderReturn = 5
    }
}
