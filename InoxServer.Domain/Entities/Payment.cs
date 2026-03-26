using InoxServer.Domain.Common;
using InoxServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public Guid OrderId { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public decimal Amount { get; set; }
        public string? TransactionId { get; set; }

        public long? PayosOrderCode { get; set; }
        public string? PayosPaymentLinkId { get; set; }
        public string? PayosCheckoutUrl { get; set; }
        public string? PayosQrCode { get; set; }
        public string? PayosWebhookData { get; set; }
        public DateTime? PayosCancelledAt { get; set; }

        public DateTime? PaidAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public Order Order { get; set; } = default!;
    }
}
