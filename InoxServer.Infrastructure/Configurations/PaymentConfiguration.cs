using InoxServer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoxServer.Domain.Enums;

namespace InoxServer.Infrastructure.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("payments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.OrderId)
                .HasColumnName("order_id")
                .IsRequired();

            builder.HasIndex(x => x.OrderId)
                .IsUnique();

            builder.Property(x => x.Method)
                .HasColumnName("method")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(PaymentStatus.Pending)
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasColumnName("amount")
                .HasColumnType("decimal(12,2)")
                .IsRequired();

            builder.Property(x => x.TransactionId)
                .HasColumnName("transaction_id")
                .HasMaxLength(200);

            builder.Property(x => x.PayosOrderCode)
                .HasColumnName("payos_order_code");

            builder.Property(x => x.PayosPaymentLinkId)
                .HasColumnName("payos_payment_link_id")
                .HasMaxLength(200);

            builder.Property(x => x.PayosCheckoutUrl)
                .HasColumnName("payos_checkout_url")
                .HasMaxLength(500);

            builder.Property(x => x.PayosQrCode)
                .HasColumnName("payos_qr_code")
                .HasMaxLength(500);

            builder.Property(x => x.PayosWebhookData)
                .HasColumnName("payos_webhook_data")
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.PayosCancelledAt)
                .HasColumnName("payos_cancelled_at");

            builder.Property(x => x.PaidAt)
                .HasColumnName("paid_at");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.PayosOrderCode);
            builder.HasIndex(x => x.PayosPaymentLinkId);

            builder.HasOne(x => x.Order)
                .WithOne(x => x.Payment)
                .HasForeignKey<Payment>(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
