using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Enums
{
    public enum PaymentMethod
    {
        Cod = 1,
        BankTransfer = 2,
        Momo = 3,
        VnPay = 4,
        ZaloPay = 5,
        PayOS = 6
    }
}
